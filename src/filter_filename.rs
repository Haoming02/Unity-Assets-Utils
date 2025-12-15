use std::io;
use std::io::Write;
use std::path::{Path, PathBuf};
use walkdir::WalkDir;

pub fn process(project_path: &Path) {
    print!("\nEnter the search string: ");
    io::stdout().flush().unwrap();

    let search_term;
    let mut search_string = String::new();

    if io::stdin().read_line(&mut search_string).is_ok() {
        search_term = search_string.trim().to_lowercase();
    } else {
        println!("Invalid Input...");
        return;
    }

    if search_term.is_empty() {
        println!("Empty Input...");
        return;
    }

    let mut matching_files: Vec<PathBuf> = Vec::new();
    let mut matching_directories: Vec<PathBuf> = Vec::new();

    for entry in WalkDir::new(project_path)
        .into_iter()
        .filter_map(|e| e.ok())
    {
        let entry_path = entry.path();

        if let Some(name) = entry_path.file_name() {
            if let Some(name_str) = name.to_str() {
                if name_str.to_lowercase().contains(&search_term) {
                    if entry_path.is_file() {
                        matching_files.push(entry_path.to_path_buf());
                    } else if entry_path.is_dir() && entry_path != project_path {
                        matching_directories.push(entry_path.to_path_buf());
                    }
                }
            }
        }
    }

    let empty = matching_files.is_empty() && matching_directories.is_empty();

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

    if empty {
        println!("\nNo result was found...");
    }
}
