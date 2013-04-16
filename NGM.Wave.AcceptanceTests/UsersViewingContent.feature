Feature: Users Viewing Content
	In order to better identify who is going to see my reply immediatly
	As a user of the Forum
	I would like to be able to see who is viewing content at the same time as myself

Scenario: Just Markus Machado is viewing the content
	Given Markus Machado has created a Thread on the Discussions Forum
	When Markus views the newly created Thread
	Then Markus's name is represented with the word 'You'

Scenario: Both Markus Machado and Vasundhara Araya are viewing the same content
	Given Markus Machado has created a Thread on the Discussions Forum
	When Markus views the newly created Thread
	Then Markus's name is represented with the word 'You'
	When Vasundhara also views the newly created Thread at the same time as Markus
	Then Vasundhara sees only his name represented as 'You' and Markus's name
	And Markus sees only his name represented as 'You' and Vasundhara's name

Scenario: Both Markus Machado and Vasundhara Araya are viewing the same content and Markus leaves
	Given Markus Machado has created a Thread on the Discussions Forum
	And both Markus and Vasundhara are viewing the same Thread
	When Markus navigates back to the discussions Forum
	Then Vasundhara should see Markus's name removed from the list of active users viewing that Thread

Scenario: Both Markus Machado and Vasundhara Araya are viewing the same content and Markus Refreshes his browser
	Given Markus and Vasundhara are viewing the Discussions Forum
	When Markus refreshes the discussions Forum
	Then Vasundhara sees only his name represented as 'You' and Markus's name
	And Markus sees only his name represented as 'You' and Vasundhara's name