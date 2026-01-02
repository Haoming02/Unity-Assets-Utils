use std::path::{Path, PathBuf};

mod byte_trimmer;
mod filter_content;
mod filter_filename;
mod flatten_folder;
mod utils;

fn title() {
    const TITLE: &'static str = r#"
  _    _       _ _                                 _         _    _ _   _ _
 | |  | |     (_) |             /\                | |       | |  | | | (_) |
 | |  | |_ __  _| |_ _   _     /  \   ___ ___  ___| |_ ___  | |  | | |_ _| |___
 | |  | | '_ \| | __| | | |   / /\ \ / __/ __|/ _ \ __/ __| | |  | | __| | / __|
 | |__| | | | | | |_| |_| |  / ____ \\__ \__ \  __/ |_\__ \ | |__| | |_| | \__ \
  \____/|_| |_|_|\__|\__, | /_/    \_\___/___/\___|\__|___/  \____/ \__|_|_|___/
                      __/ |
                     |___/
    "#;
    println!("{}\n", TITLE);
}

fn list_functions() -> String {
    println!("[0] Exit");
    println!("[1] Byte Trimmer");
    println!("[2] Flatten Folder");
    println!("[3] Filter by Content");
    println!("[4] Filter by Filename");
    return utils::prompt("Choose a Function");
}

fn get_project_path() -> PathBuf {
    loop {
        clearscreen::clear().unwrap();
        title();

        let path = utils::prompt("Enter the Path to Assets");
        let mut too_short = false;

        if path.len() < 6 {
            too_short = true;
        } else {
            let path = Path::new(path.trim());
            if path.exists() && path.is_dir() {
                return path.to_path_buf();
            }
        }

        if too_short {
            // Prevent accidentally modifying a system root folder
            println!("Path is too Short...");
        } else {
            println!("Invalid Path...");
        }

        utils::pause();
    }
}

fn main() {
    let project_path = get_project_path();

    loop {
        clearscreen::clear().unwrap();
        title();

        println!("Working Directory: {}\n", project_path.display());
        let input = list_functions();

        match input.trim().parse::<i32>() {
            Ok(choice) => match choice {
                0 => break,
                1 => byte_trimmer::process(&project_path),
                2 => flatten_folder::process(&project_path),
                3 => filter_content::process(&project_path),
                4 => filter_filename::process(&project_path),
                _ => println!("Invalid ID..."),
            },
            Err(_) => {
                println!("Invalid ID...");
            }
        }

        utils::pause();
    }
}
