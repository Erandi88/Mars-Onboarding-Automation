Feature: Skill Management
         As a Mars user,
         I want to manage skills in my profile,
         So that I can show the skills I have.

Background:
    Given I am logged in to Mars
    And I am on the Profile page
    And I am on the Skills tab

@positive
Scenario Outline: Add skill with valid details
    When I add the skill "<Skill>" with level "<Level>"
    Then the skill "<Skill>" should be displayed with level "<Level>"

Examples:
    | Skill           | Level    |
    | AutoTestingSkill | Beginner |



@positive
Scenario Outline: Edit an existing skill with valid details
    Given the skill "<Skill>" with level "<Level>" exists
    When I update the skill "<Skill>" to "<UpdatedSkill>" with level "<UpdatedLevel>"
    Then the skill "<UpdatedSkill>" should be displayed with level "<UpdatedLevel>"

Examples:
    | Skill           | Level    | UpdatedSkill             | UpdatedLevel |
    | AutoTestingSkill | Beginner | AutoCommunicationSkill   | Intermediate |



@positive
Scenario Outline: Delete an existing skill
    Given the skill "<Skill>" with level "<Level>" exists
    When I delete the skill "<Skill>"
    Then the skill "<Skill>" should be removed from the skill list

Examples:
    | Skill           | Level        |
    | AutoDeleteSkill | Intermediate |