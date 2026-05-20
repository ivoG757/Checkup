This project reached a point where the current architecture is becoming too inconsistent and hard to extend (tuple-based moves mixed with Move objects, special move handling scattered across layers).

To continue properly, it needs a refactor toward a unified Move-based system with clearer separation between:
- move generation
- move validation
- move execution
- game state management
- move translation

Paused for now so I can focus on other projects. I will return when refactoring can be done cleanly.
