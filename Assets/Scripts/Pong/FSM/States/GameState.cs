﻿using Pong.Configurations;
using Pong.FSM.States.Base;
using Pong.Managers;
using Pong.Services;
using Pong.Systems.Ball;
using Pong.Systems.Collision;
using Pong.Systems.Paddle;
using Pong.Systems.Paddle.Base;
using UnityEngine;

namespace Pong.FSM.States
{
    public class GameState : State
    {
        private readonly BallSystem _ballSystem;
        private readonly PlayerPaddleSystem _playerPaddleSystem;
        private readonly OpponentPaddleSystem _opponentPaddleSystem;
        private readonly CollisionSystem _collisionSystem;
        private readonly PongConfig _pongConfig;
        
        private bool IsPlaying { get; set; }
        private bool IsPaused { get; set; }

        public GameState(
            ConfigService configService, 
            ScreenService screenService, 
            BallSystem ballSystem, 
            PaddleSystem playerPaddleSystem,
            PaddleSystem opponentPaddleSystem,
            CollisionSystem collisionSystem,
            GameManager gameManager) : base(gameManager)
        {
            _pongConfig = configService.PongConfig;
            _ballSystem = ballSystem;
            _playerPaddleSystem = playerPaddleSystem as PlayerPaddleSystem;
            _opponentPaddleSystem = opponentPaddleSystem as OpponentPaddleSystem;
            _collisionSystem = collisionSystem;
        }
        
        public override void DoState()
        {
            switch (IsPlaying)
            {
                case true:
                    ProcessState();
                    return;
                case false:
                    ShowDependencies();
                    InitDependencies();
                    IsPlaying = true;
                    IsPaused = false;
                    break;
            }
        }

        private void ProcessState()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                IsPaused = !IsPaused;
            }

            if (IsPaused) return;
            
            
            _ballSystem.Update();
            
            _playerPaddleSystem.Update();
            _opponentPaddleSystem.Update();
            
            _collisionSystem.Update();
            
            //_playerPaddleSystem.CheckCollision();
            //_opponentPaddleSystem.CheckCollision();
            
        }

        private void InitDependencies()
        {
            _ballSystem.Reset();    
            _playerPaddleSystem.Reset();
            _opponentPaddleSystem.Reset();
        }
        
        private void ShowDependencies()
        {
            Debug.Log($"Game: {_pongConfig.victoryPoints}, {_pongConfig.initialBallSpeed}, {_pongConfig.paddleMovementSpeed}");
        }
    }
}