#include <iostream>

extern int i;

int g = 10;

void func();

int main() {
  i = 2;

  func();

  return 0;
}

void func() {
  int g = 50;

  std::cout << "I: " << i << std::endl;
  std::cout << "G: " << ::g << std::endl;
}
