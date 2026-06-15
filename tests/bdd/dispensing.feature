Feature: Medication dispensing

  Background:
    Given Morphine Sulfate is stocked in the Emergency Department Cabinet

  Scenario: Successful dispense
    Given the cabinet has 12 units on hand
    When nurse.jordan dispenses 1 unit for patient PT-1042
    Then the dispense request is accepted
    And the remaining quantity is 11
    And a MedicationDispensedEvent is published

  Scenario: Reject dispense with missing patient id
    When nurse.jordan dispenses 1 unit without a patient id
    Then the dispense request is rejected
    And the response explains that PatientId is required

  Scenario: Low-stock detection
    Given the cabinet has 12 units on hand
    And the reorder level is 10
    When nurse.jordan dispenses 2 units for patient PT-1042
    Then the remaining quantity is 10
    And a low-stock alert is generated
