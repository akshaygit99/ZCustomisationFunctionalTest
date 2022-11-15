Feature: Order Create

@akki
Scenario: Create Order
	Given I have a 'POST' API 'CreateOrders'
	And I have a json input file
	| FileName |
	| TestData/Environment/CreateOrder.json   |
	When I send API request for 'customerapi'
	Then I validate status code 200