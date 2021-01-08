# HES_634-1_CompPatterns_VSFly_WebAPI
C# WebAPI for the VSFly project

## Introduction
On the basis of the EF model worked on in the course, you must design an aircraft price management application for the airline VSFly.

## Constraints
For each flight available in the database, the other partner websites (ebooker / skyscanner type) can buy tickets for their customers through their websites as a front-end using webAPI requests from the BLL of their sites.
For each flight a base price is offered by the airline. Rules exist to maximize the filling of the aircraft and the total gain on all seats. For this there are 2 variables (the filling rate of the plane and the deadline of the flight in relation to the date of purchase of the ticket). The calculation of the sale price must be done on the WebAPI server side and be returned to the partner site on each request. In the database managed by Entity Framework, the sale price of each ticket must be saved.

## Rules
- If the airplane is more than 80% full regardless of the date:
  - sale price = 150% of the base price
- If the plane is filled less than 20% less than 2 months before departure:
  - sale price = 80% of the base price
- If the plane is filled less than 50% less than 1 month before departure:
  - sale price = 70% of the base price
- In all other cases:
  - sale price = base price

## Delivery
The result consists of 2 Visual Studio solutions.
### Partner site ([repository](https://github.com/Khrid/HES_634-1_CompPatterns_VSFly_Client))
-	With an MVC presentation layer (.net core) for
    -	List of flights
    -	Buy tickets on available flights (no change or cancellation possible)

### VSFly's WebAPI (*this repository*)
-	With a webAPI layer
  -	A controller accepting RESTfull requests and returning the data in JSON format
  - Requests to be processed:
      - [x] Return all available flights (not full)
      - [x] Return the sale price of a flight
      - [x] Buying a ticket on a flight
      - [ ] Return the total sale price of all tickets sold for a flight
      - [ ] Return the average sale price of all tickets sold for a destination (multiple flights possible)
      - [ ] Return the list of all tickets sold for a destination with the first and last name of the travelers and the flight number as well as the sale price of each ticket.
