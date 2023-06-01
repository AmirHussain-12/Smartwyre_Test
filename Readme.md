# Smartwyre Developer Test

This repository contains a solution for the Smartwyre Developer Test. It provides functionality for managing product and rebate details, as well as calculating rebates based on specific incentive types.

## Features

`ProductDataStore`: Added a `POST` method for adding product details to the data store. The method returns the identifier of the newly added product, which can be used later for rebate calculation.

`RebateDataStore`: Added a `POST` method for adding rebate details to the data store. The method returns the identifier of the newly added rebate, which is used in the calculation process.

`GetMethod`: Added code to the `GetMethod` of both RebateDataStore and ProductDataStore. These methods retrieve the identifier of the specified rebate or product, allowing it to be used in the rebate calculation process.

`UpdateMethod`: Added code to the `UpdateMethod` of `RebateDataStore` to perform the rebate calculation based on the specific incentive type.

`Constants`: Introduced constant files for code reusability throughout the Smartwyre.DeveloperTest solution.

`DB folder`: Created a `DB folder` for performing dependency injection, eliminating the need to create a new SQL connection object for every open, close, or command creation operation within the Smartwyre.DeveloperTest solution.

`Unit Tests`: Implemented test cases for each method, ensuring all tests pass within the Smartwyre.DeveloperTest.Tests solution.

`Console Application`: The Smartwyre.DeveloperTest.Runner project runs the RebateService from Program.cs as per the provided instructions.

`CalculateRebate Method`: The CalculateRebate method calculates the rebate based on the specified incentive type, as required.

`Validation`: Implemented various validations to prevent program execution from breaking due to invalid input or scenarios.

`Exception` `Handling`: Utilized exception handling to handle errors gracefully. If an error occurs, it will be caught and handled appropriately without breaking the application.

## Usage
To use this solution, follow these steps:

1. Clone the repository: git clone [https://github.com/AmirHussain-12/Smartwyre_Test.git]
2. Open the solution in your preferred development environment.
3. Build the solution to ensure all dependencies are resolved.
4. Run the console application, Smartwyre.DeveloperTest.Runner, to execute the rebate service.
5. Follow the prompts to add product and rebate details, and calculate rebates based on incentive types.

## Contributing
Contributions to this repository are welcome. If you find any issues or have suggestions for improvements, please feel free to submit a pull request.

