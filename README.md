# Part IV Project 2024

_Tristan Mona and Luca Eastwood_

## Instructions

### Specifications

The following hardware was used to operate the VR Tinnitus application:

1. Meta Quest 2 head mounted dispay (HMD) and handheld controllers.
2. Computer meeting minimum requirements for Meta Quest Link (https://www.meta.com/en-gb/help/quest/articles/headsets-and-accessories/oculus-link/requirements-quest-link/).
3. USB C 3.0 cable of suitable length to connect HMD to computer.

### How to Run

1. Clone this repository to your local machine.
2. Connect a Virtual Reality headset and controller system. The application has been developed and tested with a Meta Quest 2 headset. Other headsets will work but will likely require minor changes to the source code. See https://www.meta.com/en-gb/help/quest/articles/headsets-and-accessories/oculus-link/connect-with-air-link/ for instructions on connecting a Meta Quest 2 headset.
3. Open any of the forest, beach, blank or tutorial scenes and use the play button in Unity to run the application. Alternatively, the aforementioned scenes can be built and run as an executable using build tools in Unity.

# Research and Implementation

## Research

This iteration intends to explore the following research questions:

**RQ1: Will making the VR environment more engaging and relaxing motivate users to use it to reduce anxiety and stress?**

_We are assessing using:_

- Relaxation levels of the user using the in-application Relaxation State Questionnaire
- Motivation levels of the users using the in-application Intrinsic Motivation Inventory

**RQ2: To what degree does visual stimulation have an effect on the patients’ accurate perception of a sound source?**

_We are assessing using:_

- The angular displacement between the direction vector of the user’s aim and the direction vector of the sound source position - the audial stimulus
- The angular displacement between the direction of the user’s aim and the direction vector of the masking object (Radio) position - the visual stimulus
- Comparing the two above, how do users’ aiming accuracy change when they can see a visual stimulus (the Radio masking object) compared to when they cannot see it?

**RQ3: What design considerations are important for clinician interfaces, and will their implementations increase the usability for clinician end-users?**

_We are assessing using:_

- [Out-of-study questionnaire](https://docs.google.com/document/d/1ckg9mz-19ulEoVQRdDhDN_YLQBxtkbLp6Ma3KriSLYc/edit#heading=h.rfhtuyxj25ls) to appropriate clinical users in the Section of Audiology

## Implementation

Thus, we have made the addition of the following features:

**RQ1:**

- Forest-bathing-inspired features
  - Mindfulness/forest bathing promoting activities around the Forest environment, presented on signs
  - Seven bird species with sounds flying around the Forest and landing in two different areas
  - Additional items designed to increase engagement/interest including birdhouses
  - In-app questionnaire rating signs, which appear on clinician command, in the Tutorial, Forest and Beach worlds. They are currently configured to prompt both RSQ and two IMI sub-scale questionnaires where the user selects likert scale options after experiencing environments.

**RQ2:**

- 'Locationing Mode' Experimental set-up in Blank World for testing presence of ventriloquism
  - Patient moves to a designated point (the in-game chair), where the radio has been positioned 3m directly ahead at chest-level to a seated patient
  - Patient is prompted by clinician to aim with the controller ray to where they perceive the activated masking sound to be originating from
  - The patient presses ‘A’ to confirm their guess and audibly notifies the clinician that they have made their guess. The displacement angle (how far ‘off’ a user is) between the controller ray and the direction vector towards the true audio source is recorded. The displacement angle between the controller ray and the radio location is also recorded.
  - For each measurement recorded through patient’s button-pressing, we collect: Offset Preset Index, Radio Position Index, isRadioVisible, XY angle offset, Inclination (YZ), Displacement Angle from Audio, Displacement Angle from Radio, Audio Source Position, Controller Ray Origin point & Direction, Radio Position vector, Timestamp

**RQ3:**

- Exit button for the application.
- Logical default settings for different environments, such as turning on environment sounds automatically when entering Beach & Forest.
- Ability to toggle radio visibility on and off (enabling RQ2 features), as well as a rendering of the masking sound’s audio source position, which is a small red sphere that only the clinician can see
- In-depth toggle system for applying audio source offsets from the masking sound object, including angles in the XY and YZ planes from the centre of the object, and radius’ between 0 and 4 in-game metres.
