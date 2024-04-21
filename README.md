# Reinforcement Learning Model for Hack and Slash Game

The following is the code & reproducability instructions for the model training & game. [This](https://drive.google.com/file/d/1bEyuaE3RZvHLvkdvwxkkakb-djMVO6WX/view?usp=sharing) is the link to the full unity environment in case its needed.

# 


## Training Instructions

- Build the game into a Linux executable
- Upload the built EXECUTABLE & UnityPLayer.so into a colab environment 
- Define a .yaml CONFIG file with the training config. The following is an example of the CNN model:
```
behaviors:
  PlayerAgent:
    trainer_type: ppo
    hyperparameters:
      batch_size: 128
      buffer_size: 2048
      learning_rate: 2.0e-4
      beta: 1.0e-3 # co-efficient for PPO loss fn
      epsilon: 0.2 # clip paramater for PPO
      lambd: 0.95 # discount factot for future rewards
      learning_rate_schedule: linear
      beta_schedule: constant
      epsilon_schedule: linear
    network_settings:
      normalize: true
      hidden_units: 128
      num_layers: 3
    reward_signals:
      extrinsic:
        gamma: 0.99
        strength: 1.0
      curiosity:
        strength: 0.1
        gamma: 0.99
    max_steps: 6000000
    time_horizon: 1000
    summary_freq: 5000
```
- Run the following code to run the training procedure. This will run with 3 instances simultaneously, on GPU (cuda), and without attempting to render graphics:
```
!pip install mlagents
!chmod -R 755 /content/<EXECUTABLE>.x86_64
!chmod -R 755 /content/UnityPlayer.so
!mlagents-learn <CONFIG>.yaml --env=<EXECUTABLE> --run-id=<RUN-ID> --torch-device=cuda --num-envs=3 --no-graphics
```
