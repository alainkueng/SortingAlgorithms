# Task 2: Setup and Running Instructions

## Step 1: Install Required Software

1. **Install Visual Studio 2022**
   - **Download Link**: [Visual Studio 2022](https://visualstudio.microsoft.com/de/vs/)
   - During installation, ensure you select all necessary components, especially those for .NET development.

2. **Install Monogame**
   - Follow the setup guide for your operating system: [Monogame setup](https://docs.monogame.net/articles/getting_started/index.html)

---

## Step 2: Open the Project

### Option 1: Using the Existing Solution File
1. **Locate the Solution File**: Open the `Task_2.sln` file in **Visual Studio 2022**.
2. **Automatic Configuration**: Visual Studio should automatically adjust all properties and settings.
3. **Running the Program**:
   - For Windows users, run the included binary file (`.exe`) to start the program.

---

### Option 2: If the Solution File Fails to Load

1. **Create a New Solution File in Visual Studio**
   - Open Visual Studio 2022.
   - Go to **File** > **New** > **Project**.
   - Choose **Blank Solution**, name your solution, and click **Create**.

2. **Add the Monogame Project to the Solution**
   - In the **Solution Explorer**, right-click on the solution file.
   - Select **Add** > **Existing Project**.
   - Navigate to the folder containing the Monogame project files and select the `.csproj` file.
   - Click **Open**.

3. **Build the Project**
   - Click **Build** > **Build Solution** in the Visual Studio menu.
   - This will compile the project and generate the necessary binaries and dependencies.

4. **Run the Project**
   - Click the **Start** button in Visual Studio or press `F5`.
   - The Monogame application should launch successfully.
