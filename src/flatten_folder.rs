use std::fs;
use std::path::Path;
use walkdir::WalkDir;

use crate::utils;

pub fn process(project_path: &Path) {
    print!(
        "\n!!! Type \"YES\" to flatten folder \"{}\" !!!",
        project_path.display()
    );

    let confirmation = utils::prompt("Confirm");
    if confirmation.trim() != "YES" {
        println!("Operation Canceled...");
        return;
    }

    let mut files_to_move = Vec::new();

    for entry in WalkDir::new(project_path)
        .min_depth(2)
        .into_iter()
        .filter_map(|e| e.ok())
    {
        let path = entry.path();

        if path.is_file() {
            if let Ok(relative_path) = path.strip_prefix(project_path) {
                let new_name = relative_path
                    .to_string_lossy()
                    .replace(std::path::MAIN_SEPARATOR, "_");

                let mut destination = project_path.to_path_buf();
                destination.push(new_name);

                files_to_move.push((path.to_path_buf(), destination));
            }
        }
    }

    for (source, dest) in files_to_move {
        if let Err(e) = fs::rename(&source, &dest) {
            eprintln!("Failed to move file \"{}\"...\n\t{}", source.display(), e);
        }
    }

    for entry in WalkDir::new(project_path)
        .min_depth(1)
        .contents_first(true)
        .into_iter()
        .filter_map(|e| e.ok())
    {
        if entry.path().is_dir() {
            if let Err(e) = fs::remove_dir(entry.path()) {
                eprintln!(
                    "Failed to delete folder \"{}\"...\n\t{}",
                    entry.path().display(),
                    e
                );
            }
        }
    }

    println!("\nFolder Flattened!")
}
