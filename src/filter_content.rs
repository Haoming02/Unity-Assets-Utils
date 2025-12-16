use std::fs;
use std::io;
use std::io::Write;
use std::path::Path;
use walkdir::WalkDir;

pub fn process(project_path: &Path) {
    print!("\nEnter the search string: ");
    io::stdout().flush().unwrap();

    let search_bytes;
    let mut search_string = String::new();

    if io::stdin().read_line(&mut search_string).is_ok() {
        search_bytes = search_string.trim().as_bytes();
    } else {
        println!("Invalid Input...");
        return;
    }

    if search_bytes.is_empty() {
        println!("Empty Input...");
        return;
    }

    println!("\n[Matches]");

    let mut empty = true;

    for entry in WalkDir::new(project_path)
        .min_depth(1)
        .into_iter()
        .filter_map(|e| e.ok())
    {
        if entry.path().is_file() {
            if let Ok(file_content) = fs::read(entry.path()) {
                if contains_subslice(&file_content, search_bytes) {
                    empty = false;
                    println!("{}", entry.path().display());
                }
            }
        }
    }

    if empty {
        println!("No result was found...");
    }
}

fn contains_subslice(haystack: &[u8], needle: &[u8]) -> bool {
    assert!(!needle.is_empty());
    return haystack
        .windows(needle.len())
        .any(|window| window == needle);
}
