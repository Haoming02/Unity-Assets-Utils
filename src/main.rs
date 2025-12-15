use std::io;
use std::io::Write;
use std::path::{Path, PathBuf};

mod byte_trimmer;
mod filter_content;
mod filter_filename;
mod flatten_folder;

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
    println!("{}", TITLE);
}

fn list_functions() {
    println!("[0] Exit");
    println!("[1] Byte Trimmer");
    println!("[2] Flatten Folder");
    println!("[3] Filter by Content");
    println!("[4] Filter by Filename");
    print!("\nChoose a Function: ");
    io::stdout().flush().unwrap();
}

fn get_project_path() -> PathBuf {
    let mut project_path = String::new();
    loop {
        clearscreen::clear().unwrap();
        title();

        print!("\nEnter the Path to Assets: ");
        io::stdout().flush().unwrap();

        if io::stdin().read_line(&mut project_path).is_ok() {
            let path = Path::new(project_path.trim());
            if path.exists() && path.is_dir() {
                return path.to_path_buf();
            }
        }

        println!("Invalid Path...");
        project_path.clear();

        println!("\nPress ENTER to Continue...");
        while io::stdin().read_line(&mut project_path).is_ok() {
            if project_path.trim() == "" {
                break;
            }
            project_path.clear();
        }
    }
}

fn main() {
    let mut input = String::new();
    let project_path = get_project_path();

    loop {
        clearscreen::clear().unwrap();
        title();

        println!("Working Directory: {}\n", project_path.display());
        list_functions();

        if io::stdin().read_line(&mut input).is_ok() {
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
        } else {
            println!("Invalid ID...");
        }

        input.clear();
        println!("\nPress ENTER to Continue...");
        while io::stdin().read_line(&mut input).is_ok() {
            if input.trim() == "" {
                break;
            }
            input.clear();
        }
    }
}
