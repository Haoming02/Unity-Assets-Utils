use std::io::{self, Write};

pub fn get_input() -> String {
    let mut input = String::new();
    if io::stdin().read_line(&mut input).is_ok() {
        return input.trim().to_string();
    } else {
        return "".to_string();
    }
}

pub fn prompt(message: &str) -> String {
    print!("\n{}: ", message);
    io::stdout().flush().unwrap();
    return get_input();
}

pub fn pause() {
    println!("\nPress ENTER to Continue...");
    get_input();
}
