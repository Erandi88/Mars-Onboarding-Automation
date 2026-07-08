Feature: Skill Management
         As a Mars user,
         I want to manage skills in my profile,
         So that I can show the skills I have.

Background:
    Given I am logged in to Mars
    And I am on the Profile page
    And I am on the Skills tab

Scenario: Add skill with valid details
    When I add a new skill with valid details
    Then the skill should be displayed in the skill list

Scenario: Delete an existing skill
    Given a skill exists in the skill list
    When I delete the skill
    Then the skill should be removed from the skill list

Scenario: Edit an existing skill
    Given a skill exists in the skill list
    When I edit the skill with new valid details
    Then the updated skill should be displayed in the skill list