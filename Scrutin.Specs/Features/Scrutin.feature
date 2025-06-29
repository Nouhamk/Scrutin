Feature: Calcul du résultat d'un scrutin majoritaire
    En tant que client de l'API
    À la cloture d'un scrutin majoritaire
    Je veux calculer le résultat du scrutin
    Pour obtenir le vainqueur du vote

Scenario: Un candidat obtient plus de 50% des voix au premier tour
    Given un scrutin avec les candidats suivants
      | Name     |
      | Nouhaila |
      | Ilyass   |
      | Lina     |
    And que les votes suivants ont été enregistrés
      | Candidate | Votes |
      | Nouhaila  | 60    |
      | Ilyass    | 25    |
      | Lina      | 15    |
    When je cloture le scrutin
    Then le vainqueur devrait être "Nouhaila"
    And Nouhaila devrait avoir 60% des voix
    And le scrutin devrait être terminé en 1 tour

Scenario: Aucun candidat n'obtient plus de 50% au premier tour
    Given un scrutin avec les candidats suivants
      | Name     |
      | Nouhaila |
      | Ilyass   |
      | Lina     |
    And que les votes suivants ont été enregistrés
      | Candidate | Votes |
      | Nouhaila  | 40    |
      | Ilyass    | 35    |
      | Lina      | 25    |
    When je cloture le scrutin
    Then il devrait y avoir un second tour
    And les candidats pour le second tour devraient être "Nouhaila" et "Ilyass"

Scenario: Vainqueur au second tour
    Given un scrutin au second tour avec les candidats suivants
      | Name     | Votes |
      | Nouhaila | 55    |
      | Ilyass   | 45    |
    When je cloture le scrutin
    Then le vainqueur devrait être "Nouhaila"
    And le scrutin devrait être terminé en 2 tours

Scenario: Égalité au dernier tour - aucun vainqueur
    Given un scrutin au second tour avec les candidats suivants
      | Name     | Votes |
      | Nouhaila | 50    |
      | Ilyass   | 50    |
    When je cloture le scrutin
    Then il ne devrait pas y avoir de vainqueur
    And le scrutin devrait être terminé en 2 tours

Scenario: Égalité pour la deuxième place au premier tour
    Given un scrutin avec les candidats suivants
      | Name     |
      | Nouhaila |
      | Ilyass   |
      | Lina     |
      | Ahmed    |
    And que les votes suivants ont été enregistrés
      | Candidate | Votes |
      | Nouhaila  | 40    |
      | Ilyass    | 30    |
      | Lina      | 30    |
      | Ahmed     | 0     |
    When je cloture le scrutin
    Then il devrait y avoir un second tour
    And les candidats pour le second tour devraient être "Nouhaila", "Ilyass" et "Lina"

Scenario: Égalité parfaite entre tous les candidats au premier tour
    Given un scrutin avec les candidats suivants
      | Name     |
      | Nouhaila |
      | Ilyass   |
      | Lina     |
    And que les votes suivants ont été enregistrés
      | Candidate | Votes |
      | Nouhaila  | 33    |
      | Ilyass    | 33    |
      | Lina      | 33    |
    When je cloture le scrutin
    Then il devrait y avoir un second tour
    And tous les candidats devraient passer au second tour

Scenario: Vainqueur au premier tour avec présence de votes blancs
    Given un scrutin avec les candidats suivants
      | Name     |
      | Nouhaila |
      | Ilyass   |
      | Lina     |
    And que les votes suivants ont été enregistrés
      | Candidate | Votes |
      | Nouhaila  | 55    |
      | Ilyass    | 20    |
      | Lina      | 15    |
    And que 10 votes blancs ont été enregistrés
    When je cloture le scrutin
    Then le vainqueur devrait être "Nouhaila"
    And Nouhaila devrait avoir 55% des voix
    And il devrait y avoir 10 votes blancs
    And le total des suffrages exprimés devrait être 100

Scenario: Second tour nécessaire avec présence de votes blancs
    Given un scrutin avec les candidats suivants
      | Name     |
      | Nouhaila |
      | Ilyass   |
      | Lina     |
    And que les votes suivants ont été enregistrés
      | Candidate | Votes |
      | Nouhaila  | 40    |
      | Ilyass    | 35    |
      | Lina      | 15    |
    And que 10 votes blancs ont été enregistrés
    When je cloture le scrutin
    Then il devrait y avoir un second tour
    And les candidats pour le second tour devraient être "Nouhaila" et "Ilyass"
    And Nouhaila devrait avoir 40% des voix
    And Ilyass devrait avoir 35% des voix

Scenario: Égalité au second tour avec votes blancs
    Given un scrutin au second tour avec les candidats suivants
      | Name     | Votes |
      | Nouhaila | 45    |
      | Ilyass   | 45    |
    And que 10 votes blancs ont été enregistrés
    When je cloture le scrutin
    Then il ne devrait pas y avoir de vainqueur
    And le scrutin devrait être terminé en 2 tours
    And il devrait y avoir 10 votes blancs