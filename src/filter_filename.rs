use std::path::Path;
use walkdir::WalkDir;

use crate::utils;

pub fn process(project_path: &Path) {
    let search_term = utils::prompt("Enter the search string")
        .trim()
        .to_lowercase();

    if search_term.is_empty() {
        println!("Empty Input...");
        return;
    }

    let mut matching_files = Vec::new();
    let mut matching_directories = Vec::new();

    for entry in WalkDir::new(project_path)
        .min_depth(1)
        .into_iter()
        .filter_map(|e| e.ok())
    {
        let entry_path = entry.path();

        if let Some(name) = entry_path.file_name() {
            if let Some(name_str) = name.to_str() {
                if name_str.to_lowercase().contains(&search_term) {
                    if entry_path.is_file() {
                        matching_files.push(entry_path.to_path_buf());
                    } else if entry_path.is_dir() {
                        matching_directories.push(entry_path.to_path_buf());
                    }
                }
            }
        }
    }

    let empty = matching_files.is_empty() && matching_directories.is_empty();
    if empty {
        println!("\nNo result was found...");
        return;
    }

    if !matching_files.is_empty() {
        println!("\n[File Name]");
        for file_path in matching_files {
            println!("{}", file_path.display());
        }
    }

    if !matching_directories.is_empty() {
        println!("\n[Folder Name]");
        for dir_path in matching_directories {
            println!("{}", dir_path.display());
        }
    }
}
