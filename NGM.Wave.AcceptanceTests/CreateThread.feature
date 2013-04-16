Feature: Create a Thread
	In order to communicate his ideas with a wider audiance
	As Markus Machado
	I want to be able to create a thread

Scenario: Test
	Given I
	When T
	Then P 

Scenario: Create Thread
	Given Markus Machado has found the Discussions Forum that he would like the create a Thread on
	And Vasundhara Araya is also currently viewing the Discussions Forum
	When Markus Machado creates a new Thread
	Then he is taken to that Thread
	And Vasundhara view of the Discussions Forum is updated to include the new Thread

@wip
# need the premission to delete
Scenario: Delete Thread
	Given Markus Machado has created a Thread on the Discussions Forum
	And both Markus and Vasundhara are currently viewing the Discussions Forum
	When Markus Machado deletes the Thread
	Then the thread is removed from Markus Forum view
	And the thread is removed from Vasundhara Forum view