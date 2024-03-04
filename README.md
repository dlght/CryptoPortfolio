# Crypto Portfolio Tracker
This readme file describes a web application for calculating the value of your cryptocurrency portfolio.

## Features

### Calculate portfolio value
Upload a file containing your crypto holdings and initial purchase prices.
### File format: The file should follow the format
* Each line represents a single crypto holding.
* Lines are delimited by the pipe symbol (|).
* Each line has three values separated by pipes
  
  * Quantity: Number of coins owned (format: XXX.XXXX).
  * Coin: Name of the cryptocurrency.
  * Price: Initial purchase price per coin (format: Y.YYYY).
### Calculate changes
* Initial portfolio value: Total value of your holdings at the time of purchase.
* Individual coin change: Percentage change for each coin compared to the initial purchase price.
* Overall portfolio change: Percentage change of your entire portfolio compared to its initial value.
### Periodical updates (configurable)
Update the current portfolio value and percentage changes at a configurable interval (default: 5 minutes).
### Logging 
Log all operations (e.g., file upload, portfolio updates) to a file for record keeping.

### Note: This is a general overview of the project. 
