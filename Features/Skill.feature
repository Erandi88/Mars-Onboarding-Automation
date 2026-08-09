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


@negative @validinput
Scenario Outline: Add a duplicate skill
    Given the skill "<Skill>" with level "<Level>" exists
    When I try to add the skill "<Skill>" with level "<Level>" again
    Then only one "<Skill>" with level "<Level>" should exist
    And the duplicate skill message should be displayed

Examples:
    | Skill              | Level    |
    | AutoDuplicateSkill | Beginner |


@negative @validinput
Scenario Outline: Update a skill to an existing skill
    Given the skill "<ExistingSkill>" with level "<ExistingLevel>" exists
    And the skill "<SkillToUpdate>" with level "<CurrentLevel>" exists
    When I try to update the skill "<SkillToUpdate>" to "<ExistingSkill>" with level "<ExistingLevel>"
    Then the skill already added message should be displayed
    When I cancel the skill edit
    Then the skill "<ExistingSkill>" should be displayed with level "<ExistingLevel>"
    And the skill "<SkillToUpdate>" should be displayed with level "<CurrentLevel>"

Examples:
    | ExistingSkill    | ExistingLevel | SkillToUpdate          | CurrentLevel |
    | AutoTestingSkill | Beginner      | AutoCommunicationSkill | Intermediate |


@negative @invalidinput
Scenario Outline: Add a skill with an empty skill field
    When I try to add a skill with an empty skill field and level "<Level>"
    Then the skill validation message should be displayed
    And no new skill record should be created

Examples:
    | Level    |
    | Beginner |


@negative @invalidinput
Scenario Outline: Add a skill with an empty level field
    When I try to add the skill "<Skill>" with an empty level
    Then the skill validation message should be displayed
    And no new skill record should be created

Examples:
    | Skill               |
    | AutoEmptyLevelSkill |


@negative @validinput
Scenario Outline: Add the same skill with a different level
    Given the skill "<Skill>" with level "<CurrentLevel>" exists
    When I try to add the skill "<Skill>" with level "<NewLevel>" again
    Then only one record for the skill "<Skill>" should exist
    And the skill "<Skill>" should be displayed with level "<CurrentLevel>"
    And the skill duplicated data message should be displayed

Examples:
    | Skill            | CurrentLevel | NewLevel     |
    | AutoTestingSkill | Beginner     | Intermediate |


@negative @invalidinput
Scenario Outline: Update a skill with an empty skill field
    Given the skill "<Skill>" with level "<Level>" exists
    When I try to update the skill "<Skill>" with an empty skill field
    Then the skill validation message should be displayed
    When I cancel the skill edit
    Then the skill "<Skill>" should be displayed with level "<Level>"

Examples:
    | Skill            | Level    |
    | AutoTestingSkill | Beginner |


@destructive
Scenario Outline: Add a skill with a very large input
    When I add a skill containing "<CharacterCount>" characters with level "<Level>"
    Then the very large skill should be displayed with level "<Level>"
    And the very large skill record should remain interactive

Examples:
    | CharacterCount | Level    |
    | 500            | Beginner |