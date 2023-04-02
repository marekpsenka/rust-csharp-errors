#[cfg(test)]
mod tests {
    #[test]
    #[ignore = "avoid blocking test run by input"]
    fn build_me_and_get_a_warning() {
        let mut s = String::new();
        std::io::stdin().read_line(&mut s);
        println!("{s}");
    }
}
