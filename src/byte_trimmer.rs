use std::fs;
use std::io;
use std::path::Path;
use walkdir::WalkDir;

const HEADER: &'static [u8] = b"UnityFS";
const THRESHOLD: usize = 2048;

pub fn process(project_path: &Path) {
    for entry in WalkDir::new(project_path)
        .min_depth(1)
        .into_iter()
        .filter_map(|e| e.ok())
    {
        if entry.path().is_file() {
            if let Err(e) = trim_file(entry.path()) {
                eprintln!(
                    "Failed to process \"{}\"...\n\t{}",
                    entry.path().display(),
                    e
                );
            }
        }
    }

    println!("\nHeaders Fixed!");
}

fn trim_file(path: &Path) -> io::Result<()> {
    let data = fs::read(path)?;

    let search_range = std::cmp::min(data.len(), THRESHOLD);
    let search_area = &data[..search_range];

    if let Some(index1) = find_subslice(search_area, HEADER) {
        let start_after_first = index1 + HEADER.len();
        let index2 = if start_after_first < search_area.len() {
            find_subslice(&search_area[start_after_first..], HEADER).map(|i| i + start_after_first)
        } else {
            None
        };

        let final_index = index2.unwrap_or(index1);
        if final_index > 0 {
            fs::write(path, &data[final_index..])?;
        }
    } else {
        println!("\tNo Header Found! Skipping \"{}\"", path.display());
    }

    Ok(())
}

fn find_subslice(haystack: &[u8], needle: &[u8]) -> Option<usize> {
    return haystack
        .windows(needle.len())
        .position(|window| window == needle);
}
