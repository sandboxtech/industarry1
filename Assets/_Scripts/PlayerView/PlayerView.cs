 
using System;
using System.Collections.Generic;
using UnityEngine;

namespace W
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer PlayerSpriteRenderer;
        public SpriteRenderer SpriteRenderer => PlayerSpriteRenderer;

        [SerializeField]
        private SpriteRenderer PlayerSpriteShadowRenderer;


        private Transform PlayerSpriteTransform;

        public static PlayerView I { get; private set; }

        private void Awake() {
            A.Assert(I == null);
            I = this;

            PlayerSpriteTransform = PlayerSpriteRenderer.transform;
        }

        public const float CameraSpeed = 15;


        private Sprite[] skin_buffer;
        private void Start() {
            skin_buffer = new Sprite[C.PlayerSpriteHeight * C.PlayerSpriteWidth];
            BindSkinBuffer("player");

        }

        public void BindSkinBuffer(string keyBase) {
            for (int i = 0; i < skin_buffer.Length; i++) {
                skin_buffer[i] = Res.I.SpriteOf(StrPool.Concat(keyBase, i));
            }
        }


        public void RerenderPlayer() {

            if (G.terrain.NoPlayerAvatar) {
                PlayerSpriteRenderer.enabled = false;
                PlayerSpriteShadowRenderer.enabled = false;
            } else {
                PlayerSpriteRenderer.enabled = true;
                PlayerSpriteShadowRenderer.enabled = true;
            }
        }

        private void Update() {
            if (G.terrain.NoPlayerAvatar) {
                // ?
            }
            else {
                (G.player as __Player__).UpdateState();

                PlayerSpriteTransform.position = PlayerBottomLeft + new Vector3(0, -C.PPUReciprocal, 0);
                SyncPlayerAnimation();
            }
        }

        private const int eatingRow = 1;
        private const int directionRow = 2;
        private const int interactionRow = 6;
        private void SyncPlayerAnimation() {
            Player player = G.player;
            int index;
            int directionNumber = Player.DirectionNumber(player.direction); // 0 1 2 3 代表 右上左下

            switch (player.state.Type) {

                case PlayerStateType.eating:
                    index = eatingRow * C.PlayerSpriteWidth;
                    index += (int)(player.state.ActionProgress * C.PlayerEatingSpriteWidth) % C.PlayerEatingSpriteWidth;
                    break;

                case PlayerStateType.walking:
                    const int frameCount = 6;
                    const float cyclePerTile = 0.25f;
                    index = (directionRow + directionNumber) * C.PlayerSpriteWidth;
                    float tileCount = player.state.step_float_since_walk_start;
                    index += (int)(tileCount * cyclePerTile * frameCount) % frameCount;
                    break;

                case PlayerStateType.interacting:
                    index = (interactionRow + directionNumber) * C.PlayerSpriteWidth;
                    index += (int)(player.state.ActionProgress * C.PlayerInteractionSpriteWidth) % C.PlayerInteractionSpriteWidth;
                    break;

                case PlayerStateType.idle:
                    index = (2 + directionNumber) * C.PlayerSpriteWidth;
                    break;

                default:
                    throw new Exception();
            }
            PlayerSpriteRenderer.sprite = skin_buffer[index];
            return;
        }

        public Quaternion PlayerRotation {
            get {
                return Quaternion.Euler(0, 0, -90 + Player.DirectionNumber(G.player.direction) * 90);
            }
        }
        public Vector3 PlayerFrontOf(float t) {
            int y = G.player.direction.y;
            if (y == 1) y = 3;
            return PlayerBottomLeft + new Vector3(0.5f + t * G.player.direction.x, 0.5f + t * y, 1); ;
        }
        public Vector3 PlayerCenter => PlayerBottomLeft + new Vector3(0.5f, 0.5f, 1);
        private Vector3 playerBottomLeftBuffer;
        private long playerBottomLeftFrame;
        public Vector3 PlayerBottomLeft {
            get {
                return Information.FrameBuffer<Vector3>(ref playerBottomLeftBuffer, ref playerBottomLeftFrame, () => {
                    Player player = G.player;

                    Vector3 position = new Vector3(G.player.position.x, G.player.position.y, 0);
                    Vector3 result;

                    switch (player.state.Type) {
                        case PlayerStateType.walking:
                            float t = player.state.ActionProgress;
                            A.Assert(t >= 0 && t <= 1, () => t.ToString());
                            Vector3 offset = (Vector3)(player.direction) * (1 - t);
                            result = position - offset;
                            break;
                        default:
                            result = position;
                            break;
                    }
                    result = new Vector3(
                        M.Floor(result.x * C.PixelPerUnit) * C.PPUReciprocal,
                        M.Floor(result.y * C.PixelPerUnit) * C.PPUReciprocal,
                        result.z);
                    return result;
                });
            }
        }
    }
}
