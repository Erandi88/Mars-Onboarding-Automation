Feature: Mars Login Functionality
    As a registered Mars user,
    I want to log in to the application,
    So that I can access my profile and manage my languages and skills.

Scenario: Login successfully with valid credentials
    Given I am on the Mars home page
    When I open the Sign In form
    And I enter valid Mars credentials
    And I click the Login button
    Then I should be logged in successfully