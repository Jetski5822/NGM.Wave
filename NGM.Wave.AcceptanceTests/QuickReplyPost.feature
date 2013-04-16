Feature: Post
	In order to answer replied to a Thread in a speedy manner
	As Vasundhara Araya
	I would like to be able to reply to posts without leaving the page

Scenario: Vasundhara Araya replied to a post created by Markus Machado
	Given Markus Machado has created a Thread on the Discussions Forum
	And Markus is currently viewing the Discussions Forum
	When Vasundhara replies to Markus's question
	Then Vasundhara see the reply added
	And Markus sees details on that Thread be updated in the Discussions Forum 
