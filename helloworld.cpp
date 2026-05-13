#include <iostream>

#define CONST1 10

const int const2 = 20;

int arr[5] = {1, 2, 3, 4, 5};

int main() {
  std::cout << "Hello, World!" << std::endl;
  std::cout << "CONST1: " << CONST1 << std::endl;
  std::cout << "const2: " << const2 << std::endl;

  std::cout << "Array elements: ";
  for (int i = 0; i < 5; ++i) {
    std::cout << arr[i] << " ";
  }
  std::cout << std::endl;
  
  return 0;
}
