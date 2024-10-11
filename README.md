# Swipey Cars

**Swipey Cars** is an exciting hyper-casual game designed for Android where the player's swiping precision determines success. The objective is simple: swipe the screen to guide a car from point A to point B, but there's a catch—swipe too hard or too softly, and you lose. The challenge lies in finding that perfect balance for smooth driving.

## Project Overview

Swipey Cars is currently in development and will soon be available on the Google Play Store and itch.io. The game promises engaging, fast-paced gameplay and features various elements to enhance the player's experience.

### Key Features
- **Multiple Levels**: Play through an increasing number of levels, each more difficult than the last, ensuring that players are constantly engaged.
- **Custom In-Game Economy**: Build your virtual wealth through coins and rewards, and spend them on in-game items.
- **Advertisements**: Integrated Unity Ads for monetization, including banner ads, interstitial ads, and rewarded video ads to enhance player interaction.
- **In-App Purchases**: Players can buy in-game items or currency through a shop with real money, offering an additional layer of customization and progress.
- **Push Notifications**: Stay informed with timely notifications that remind players of game events, special offers, and upcoming challenges.

### Backend and Data Management
Swipey Cars features a custom-built backend from scratch using Python, ensuring efficient and secure handling of:
- **User Data**: Storing player information such as login credentials, progress, and in-game purchases.
- **Authentication**: Seamless sign-up and login mechanisms, ensuring user data security.
- **Leaderboards**: A competitive leaderboard system using Redis' sorted sets, allowing players to compare scores and rankings globally.

## Development Details

As a solo developer, I am utilizing a modern tech stack to create an efficient and scalable game:

### Tech Stack
- **Unity**: The core engine for building game mechanics, levels, and animations.
- **C#**: The main language for writing game logic, managing physics, and handling player interactions.
- **Python (FastAPI)**: Used for developing the backend API. FastAPI's asynchronous nature ensures fast response times and scalability for handling user authentication and leaderboard data.
- **Redis**: A powerful, in-memory database, perfect for fast, real-time leaderboard management and caching of critical game data.
- **Supabase**: A managed PostgreSQL solution used for storing persistent player data such as profiles, progress, and in-app purchases.
- **Firebase**: Integrated to track in-game analytics, monitor player behavior, and send push notifications to re-engage players.

This project serves both as a learning experience and as the thesis for my BASc in Game Development. While the graphics may not be perfect, it's important to note that I am primarily a software developer and not a digital artist. All assets are sourced from [Kenney.nl](https://kenney.nl/assets) and are free to use.

## Future Plans

After launching the initial Android version, I will gather feedback from early users and iterate quickly to improve the game. Planned future enhancements include:
- **iOS Release**: A potential iOS version based on the feedback and success of the Android release.
- **Real-Time Chat**: Implementing a Pub/Sub system using Redis to allow players to chat in real time during gameplay.
- **More Content**: Additional levels, challenges, and in-game items to expand the game's content over time.