# 2D Radar Object Tracking

This project simulates and visualizes the tracking of multiple objects in a two-dimensional plane using a simplified radar model.

## Description

Using radar data, the project generates 2D images in which each pixel has a value from 0 to 255, corresponding to the strength of the reflected signal. Noise and object reflections are simulated, and the main goal is to detect and track moving targets.

The project implements the following functionalities:

- **Radar data generation:** background noise and objects with a specified radius and intensity.
- **Image thresholding using the Otsu method:** automatic thresholding for object segmentation.
- **Blob extraction:** Connected Component Labeling (CCL) algorithm groups pixels belonging to the same object.
- **Blob statistics:** calculation of mean coordinates and standard deviations of pixels within blobs, modeling positions as a normal distribution.
- **Interactive Visualization:** Track objects on a WPF canvas with color-coded tracks and ellipses representing positional uncertainty.
- **Manual track addition:** users can click on the canvas to create new tracked objects.

## Technologies

- **C# / .NET 9.0**
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
4. You can also run ready to go exe file in publish folder ([see](publish))

## Demo Features

- Dynamic object generation and tracking in real time.
- Visualization of position and standard deviation (ellipses).
- Manual addition of tracked objects by clicking on the canvas.
- Color-coding of each track.

## Visual Presentation

![image](/task/image.png)

## Challenges and Limitations

- The simulation is limited to a 2D model; in real radar systems, additional dimensions and motion models are considered.
- The probabilistic association tree can become heavy with a large number of objects and measurements.
