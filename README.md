# AR Indoor Navigation

A Unity-based **Augmented Reality (AR)** prototype for indoor navigation.  
It overlays 3D markers in real space, guiding users through indoor environments using AR.

---

## Features

- Real-time AR pathing and wayfinding   
- Spatial alignment between virtual and physical space  
- Built with Unity’s AR Foundation (supports ARKit & ARCore)

---

## Setup & Installation

1. Clone the repository:  
   ```bash
   git clone https://github.com/MrNerix/ar-indoor-navigation.git
   ```

2. Open the project in **Unity Hub** → **Add project from disk** → select `AR Indoor Navigation`.

3. Install required Unity packages:
   - **AR Foundation**
   - **ARCore XR Plugin** (Android)
   - **ARKit XR Plugin** (iOS)

4. Open the main scene (`MainScene.unity`) and hit **Play** or build to your AR-capable device.

> Recommended Unity Version: **2021.3 LTS** or newer.

---

## Project Structure

```
Assets/
 ├── Scripts/      # Navigation & AR logic
 ├── Prefabs/      # Arrows, markers, UI
 ├── Scenes/       # Main AR scene
 └── Materials/    # Visual elements
```
*This project explores the intersection of AR, navigation, and user experience.*
