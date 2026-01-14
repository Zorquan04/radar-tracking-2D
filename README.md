# 2D Radar Object Tracking

This project simulates and visualizes the tracking of multiple objects in a two-dimensional plane using a simplified radar model.

## Description

Using radar data, the project generates 2D images in which each pixel has a value from 0 to 255, corresponding to the strength of the reflected signal. Noise and object reflections are simulated, and the main goal is to detect and track moving targets.

The project implements the following functionalities:

- **Radar data generation:** background noise and objects with a specified radius and intensity.
- **Image thresholding using the Otsu method:** automatic thresholding for object segmentation.
- **Blob extraction:** Connected Component Labeling (CCL) algorithm groups pixels belonging to the same object.
- **Blob statistics:** calculation of mean coordinates and standard deviations of pixels within blobs, modeling positions as a normal distribution.
- **Probabilistic Association Tree:** A data structure for managing various assignment scenarios.
- **Interactive Visualization:** Track objects on a WPF canvas with historical trajectories, position prediction, and deviation visualization.

## Technologies

- **C# / .NET 9**
- **WPF** – interactive 2D visualization
- **NUnit** – unit testing
- Data structures and algorithms: Connected Component Labeling, Gaussian Distribution, Motion Model

## Project Structure

- `RadarTracking2D.App` - entry point to a WPF application.

- `RadarTracking2D.Core` – object simulation, segmentation, and tracking logic.

- `RadarTracking2D.WPF` – user interface and visualization.

- `Tests` – unit tests for thresholding (Otsu), CCL, and tracking.

## Launch

1. Open the project in Visual Studio 2022 (or later).
2. Launch the WPF project to see an interactive simulation.
3. To verify the algorithms' performance, run NUnit tests in the `Tests` project.

## Demo Features

- Dynamic object generation and tracking in real time.
- Visualization of position, standard deviation (ellipse), and predicted trajectories.
- Object movement history and color-coding of each track.

## Visual Presentation

![image](/task/image.png)

## Challenges and Limitations

- The MHT algorithm can saturate with a large number of objects and measurements.
- The simulation is limited to a 2D model; in real radar systems, additional dimensions and motion models are considered.
