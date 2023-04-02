pub fn add(left: usize, right: usize) -> usize {
    left + right
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn it_works() {
        let result = add(2, 2);
        assert_eq!(result, 4);
    }

    #[test]
    fn build_me_and_get_a_warning() {
        let mut s = String::new();
        std::io::stdin().read_line(&mut s);
        println!("{s}");
    }
}
