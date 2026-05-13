Rocket Tycoon Calculator - Command Line Edition
===============================================

Game Type: Pure text-based incremental / idle clicker game.
Platform: Console / Terminal (100% text, no graphics).

Core Concept:
You run a small rocket company. Start with very little money and grow your business by earning income, upgrading your factory and staff, and launching rockets. Every action requires calculations using arithmetic concepts from Section 1.

Win Condition:
There is no end. The goal is to grow as much as possible — reach very high money values, launch many successful rockets, and achieve high success rates.

=== Core Gameplay Rules ===

1. Earning Money
   - Manual Click: Earn immediate money by working on rocket parts.
   - Passive Income: Earn money automatically every second based on your factory level and number of engineers.

2. Upgrades
   - Factory: Increases passive income significantly. Cost grows exponentially.
   - Engineers: Further boosts passive income. Cost grows exponentially.
   - Rocket: Improves launch success rate and rewards. Cost grows exponentially.

3. Rocket Launches (Main Feature)
   Before each launch the game displays:
   - Distance to target
   - Required speed (uses square root)
   - Time to orbit (distance / speed)
   - Fuel needed
   - Success probability (affected by rocket level, failed launches, and weather cycle)

   Launches cost money. Success gives large rewards. Failure increases penalties and reduces future success chance.

=== Math Topics Actively Used ===

- Basic operations (+, -, ×, ÷) and BODMAS order of operations
- Rounding of all costs and results
- Exponents and powers (upgrade costs and income scaling)
- Square roots (rocket speed)
- Logarithms (launch rewards)
- Remainders / Modulo (%) for weather cycle (repeating pattern)
- Distance, Speed, and Time formulas
- Rates of change (income per second)
- Scientific notation for very large money values
- Graphs via simple text-based bars (income trend over time)

=== Command List (Displayed in Game) ===

1 → Manual Click (earn money)
2 → Buy Factory Upgrade
3 → Hire Engineer
4 → Upgrade Rocket
5 → Launch Rocket
0 → Quit Game

=== Additional Rules ===

- All costs and rewards are rounded to whole numbers or 2 decimal places.
- Upgrade costs increase exponentially as levels rise.
- Weather affects launch success on a repeating cycle (using modulo).
- Late-game money values switch to scientific notation.
- Income per second and income trend (text graph) are always visible to help the player understand rates of change.

Game Style:
Pure command-line. All output through print statements. All input through keyboard (numbers 0-5).

Recommended Development Order:
1. Variables and status display
2. Main game loop with timer for passive income
3. Manual earning and upgrade purchases
4. Full rocket launch system with all calculations
5. Polish, balancing, and extra math features

This game is designed so that implementing and playing it naturally exercises every major topic from Section 1 of the Math for Games course.