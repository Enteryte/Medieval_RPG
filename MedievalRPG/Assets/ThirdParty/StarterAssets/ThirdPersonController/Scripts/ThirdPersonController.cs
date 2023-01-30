using UnityEngine;
using System.Collections;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using Cinemachine;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        public static ThirdPersonController instance;

        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float normalMoveSpeed = 2;
        public float MoveSpeed = 2.0f;

        [Tooltip("Sprint speed of the character in m/s")]
        public float SprintSpeed = 5.335f;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f;
        public float JumpHeightWithWeight = 0.8f; 

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true;

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        public float lookSensitivity;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
#endif
        /*[HideInInspector] */public Animator _animator;
        private CharacterController _controller;
        [HideInInspector] public StarterAssetsInputs _input;
        private GameObject _mainCamera;
        public CinemachineVirtualCamera _normalVCamera;
        public CinemachineVirtualCamera _bowAimingVCamera;
        public CinemachineVirtualCamera _bowAimingZoomVCamera;

        public bool canMove = true;

        private const float _threshold = 0.01f;

        [Header("Stamina")]
        public float runStaminaReduceValue;
        public float rollStaminaReduceValue;
        public float jumpStaminaReduceValue;
        public float normalAttackStaminaReduceValue;
        public float heavyAttackStaminaReduceValue;

        [Header("Attacking")]
        public int attackClicks = 0;

        [Header("Roll")]
        //[SerializeField] AnimationCurve rollCurve;
        public bool isRolling = false;
        float rollTimer;
        public CharacterController characterController;
        public AnimationClip rollAnim;
        bool rollStopMoving = false;

        public Transform currSeatTrans;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
            }
        }

        private void Awake()
        {
            //if (instance == null)
            //{
                instance = this;

                _hasAnimator = true;

                // get a reference to our main camera
                if (_mainCamera == null)
                {
                    _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                }

            //    //GameManager.instance.playerGO = this.gameObject;

            //    DontDestroyOnLoad(this.gameObject.transform.parent.parent.gameObject);

            //    DontDestroyOnLoad(_bowAimingVCamera);
            //    DontDestroyOnLoad(_bowAimingZoomVCamera);
            //    DontDestroyOnLoad(_normalVCamera);
            //    DontDestroyOnLoad(_mainCamera);
            //}
            //else
            //{
            //    //GuessTheCardMinigameManager.instance.gTCUI = this.gTCMM.gTCUI;
            //    //PrickMinigameManager.instance.prickUI = this.pMM.prickUI;

            //    Destroy(_bowAimingVCamera);
            //    Destroy(_bowAimingZoomVCamera);
            //    Destroy(_normalVCamera);
            //    Destroy(_mainCamera);

            //    Destroy(this.gameObject.transform.parent.parent.gameObject);
            //}
        }

        private void Start()
        {
            if (instance == this)
            {
                _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

                //_hasAnimator = TryGetComponent(out _animator);
                _controller = GetComponent<CharacterController>();
                _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
                _playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

                AssignAnimationIDs();

                // reset our timeouts on start
                _jumpTimeoutDelta = JumpTimeout;
                _fallTimeoutDelta = FallTimeout;

                //Keyframe roll_lastFrame = rollCurve[rollCurve.length - 1];
                //rollTimer = roll_lastFrame.time;

                //Keyframe slowedRoll_lastFrame = slowedRollCurve[slowedRollCurve.length - 1];
                //slowedRollTimer = slowedRoll_lastFrame.time;
            }
        }

        private void Update()
        {
            if (_animator.GetBool("DoPush") && !SceneChangeManager.instance.wentThroughTrigger)
            {
                return;
            }

            if (_animator.GetBool("GrabItem"))
            {
                return;
            }

            //Debug.Log(_animator.speed);

            //_hasAnimator = TryGetComponent(out _animator);

            if (!GameManager.instance.gameIsPaused)
            {
                if (canMove && currSeatTrans == null)
                {
                    JumpAndGravity();
                    GroundedCheck();

                    if (!_animator.GetBool("Roll")/* && !_animator.GetBool("HeavyAttack") && attackClicks == 0*/)
                    {
                        Move();
                    }

                    if (_animator.GetBool("Grounded"))
                    {
                        /*if (!_animator.GetBool("Bow_Aim"))
                        {
                            if (Input.GetKeyDown(KeyCode.Mouse0) && attackClicks < 3)
                            {
                                if (attackClicks > 0 && PlayerValueManager.instance.currStamina - (normalAttackStaminaReduceValue * attackClicks) > 0
                                    || attackClicks == 0 && PlayerValueManager.instance.currStamina - normalAttackStaminaReduceValue > 0)
                                {
                                    //if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
                                    //{
                                    //    _animator.speed = 0.6f;
                                    //}

                                    attackClicks += 1;
                                    _animator.SetInteger("AttackClicks", attackClicks);

                                    if (_animator.GetLayerWeight(1) != 0.5f)
                                    {
                                        _animator.SetLayerWeight(1, 0.5f);
                                    }
                                }
                            }

                            if (Input.GetKeyDown(KeyCode.Mouse1) && _animator.GetBool("Bow"))
                            {
                                _animator.SetBool("Bow_Aim", true);
                                HandleBowAimingCameras(_bowAimingVCamera, _normalVCamera, _bowAimingZoomVCamera);
                            }
                            else if (Input.GetKeyDown(KeyCode.Mouse1) && !_animator.GetBool("Bow") && PlayerValueManager.instance.currStamina - heavyAttackStaminaReduceValue > 0)
                            {
                                //if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
                                //{
                                //    _animator.speed = 0.6f;
                                //}

                                _animator.SetBool("HeavyAttack", true);

                                //if (_animator.GetLayerWeight(1) != 0.5f)
                                //{
                                //    _animator.SetLayerWeight(1, 0.5f);
                                //}
                            }

                            if (_animator.GetBool("Grounded") && attackClicks == 0)
                            {
                                if (Input.GetKeyDown(KeyCode.Tab) && !isRolling && PlayerValueManager.instance.currStamina - rollStaminaReduceValue > 0)
                                {
                                    if (DebuffManager.instance.slowPlayerDebuff)
                                    {
                                        _animator.speed = 1;

                                        StartCoroutine(SlowedRoll());
                                    }
                                    else
                                    {
                                        StartCoroutine(Roll());
                                    }
                                }
                            }

                            //if (_animator.GetBool("Roll"))
                            //{
                            //    this.gameObject.transform.forward += new Vector3(1, 0, 1);
                            //}
                        }
                        else
                        {
                            if (Input.GetKeyDown(KeyCode.Mouse0) && PlayerValueManager.instance.currStamina - normalAttackStaminaReduceValue > 0)
                            {
                                if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
                                {
                                    _animator.speed = 0.6f;
                                }

                                _animator.SetBool("Bow_Shoot", true);
                            }

                            if (Input.GetKeyDown(KeyCode.Mouse1))
                            {
                                _animator.SetBool("Bow_Aim", false);
                                HandleBowAimingCameras(_normalVCamera, _bowAimingVCamera, _bowAimingZoomVCamera);
                            }

                            if (Input.GetKeyDown(KeyCode.Mouse2))
                            {
                                if (_bowAimingZoomVCamera.Priority == 12)
                                {
                                    HandleBowAimingCameras(_bowAimingVCamera, _bowAimingZoomVCamera, _normalVCamera);
                                }
                                else
                                {
                                    HandleBowAimingCameras(_bowAimingZoomVCamera, _bowAimingVCamera, _normalVCamera);
                                }
                            }

                            if (_animator.GetBool("Grounded") && !_animator.GetBool("Bow_Shoot"))
                            {
                                //if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling)
                                //{
                                //    HandleBowAimingCameras(_normalVCamera, _bowAimingVCamera, _bowAimingZoomVCamera);
                                //    _animator.SetBool("Bow_Aim", false);
                                //    StartCoroutine(Roll());
                                //}

                                if (_animator.GetBool("Grounded") && !isRolling && attackClicks == 0)
                                {
                                    if (Input.GetKeyDown(KeyCode.Tab) && !isRolling && PlayerValueManager.instance.currStamina - rollStaminaReduceValue > 0)
                                    {
                                        HandleBowAimingCameras(_normalVCamera, _bowAimingVCamera, _bowAimingZoomVCamera);
                                        _animator.SetBool("Bow_Aim", false);

                                        //if (DebuffManager.instance.slowPlayerDebuff)
                                        //{
                                        //    //DebuffManager.instance.slowPlayerDebuff = false;
                                        //    _animator.speed = 1;

                                        //    StartCoroutine(SlowedRoll());
                                        //}
                                        //else
                                        //{
                                            StartCoroutine(Roll());
                                        //}
                                    }
                                }
                            }
                        }*/
                    }
                }
            }           
        }

        private void LateUpdate()
        {
            if (!GameManager.instance.gameIsPaused && canMove && !SceneChangeManager.instance.wentThroughTrigger)
            {
                CameraRotation();
            }
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        public void StopMovingWhileRolling()
        {
            rollStopMoving = false;
        }

        public IEnumerator Roll()
        {
            isRolling = true;

            if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
            {
                _animator.speed = 0.6f;
            }

            PlayerValueManager.instance.RemoveStamina(rollStaminaReduceValue);

            //_animator.SetBool("Roll", true);

            float timer = 0;

            rollStopMoving = true;

            while (rollStopMoving)
            {
                float speed = _animator.speed * 2;

                Vector3 dir = Vector3.zero;

                //if (DebuffManager.instance.slowPlayerDebuff)
                //{
                //    dir = ((transform.forward * (speed - DebuffManager.instance.slowedFightSpeed) * 2));
                //}
                //else
                //{
                    dir = ((transform.forward * speed * 2));
                //}

                characterController.Move(dir * Time.deltaTime);

                timer += Time.deltaTime;

                yield return null;
            }

            //_animator.SetBool("Roll", false);

            yield return new WaitForSeconds(0.4f);

            _animator.speed = 1f;

            isRolling = false;
        }

        //IEnumerator SlowedRoll()
        //{
        //    isRolling = true;

        //    PlayerValueManager.instance.RemoveStamina(rollStaminaReduceValue);

        //    _animator.SetBool("Roll", true);

        //    float timer = 0;

        //    while (timer < slowedRollTimer)
        //    {
        //        float speed = rollAnim.length;

        //        Vector3 dir = Vector3.zero;

        //        //if (DebuffManager.instance.slowPlayerDebuff)
        //        //{
        //        //    dir = ((transform.forward * (speed - DebuffManager.instance.slowedFightSpeed) * 2));
        //        //}
        //        //else
        //        //{
        //        dir = ((transform.forward * /*((*/speed * 2/*) / 2)*/));
        //        //}

        //        characterController.Move(dir * Time.deltaTime);

        //        timer += Time.deltaTime;

        //        yield return null;
        //    }

        //    _animator.SetBool("Roll", false);

        //    if (DebuffManager.instance.slowPlayerDebuff)
        //    {
        //        _animator.speed = 0.5f;
        //    }

        //    yield return new WaitForSeconds(0.4f);

        //    isRolling = false;
        //}

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            //if (FightManager.instance.currTargetEnemy == null)
            //{
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * lookSensitivity;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * lookSensitivity;
            }

            // clamp our rotations so our values are limited 360 degrees

            if (_normalVCamera.m_Priority >= 12)
            {
                _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);
            }
            else
            {
                _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
                _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, -20, 20);
            }

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
            //}
        }

        private void Move()
        {
            // set target speed based on move speed, sprint speed and if sprint is pressed
            float targetSpeed;

            if (!_animator.GetBool("Bow_Aim") && PlayerValueManager.instance.currStamina - runStaminaReduceValue > 0 && !DebuffManager.instance.slowPlayerDebuff
                && InventoryManager.instance.currHoldingWeight <= InventoryManager.instance.maxHoldingWeight && !_animator.GetBool("UsingHBItem"))
            {
                //if (DebuffManager.instance.slowPlayerDebuff)
                //{
                //    targetSpeed = _input.sprint ? DebuffManager.instance.slowedSprintSpeed : DebuffManager.instance.slowedMoveSpeed;
                //}
                //else
                //{
                    targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
                //}

                if (_input.sprint && _input.move != Vector2.zero)
                {
                    PlayerValueManager.instance.RemoveStamina(runStaminaReduceValue);
                }
            }
            else
            {
                //if (DebuffManager.instance.slowPlayerDebuff)
                //{
                //    targetSpeed = DebuffManager.instance.slowedMoveSpeed;
                //}
                //else
                //{
                    targetSpeed = MoveSpeed;
                //}
            }

            // a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

            // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is no input, set the target speed to 0
            if (_input.move == Vector2.zero || attackClicks > 0 || _animator.GetBool("HeavyAttack")) targetSpeed = 0.0f;

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (_input.move != Vector2.zero || FightingActions.instance.aims == true)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y /*+ 5*/, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                float rotationX = 0;

                if(FightingActions.instance.aims == true)
                {
                    rotationX = _mainCamera.transform.eulerAngles.x;
                }

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(rotationX, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            if (attackClicks > 0 || _animator.GetBool("HeavyAttack"))
            {
                targetDirection = Vector3.zero;
                _verticalVelocity = 0;
            }

            // move the player
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);

                if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
                {
                    _animator.SetFloat(_animIDMotionSpeed, 2);
                }
                else
                {
                    _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
                }
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded && !_animator.GetBool("Roll") && !_animator.GetBool("HeavyAttack") && attackClicks == 0)
            {
                // reset the fall timeout timer
                _fallTimeoutDelta = FallTimeout;

                // update animator if using character
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // stop our velocity dropping infinitely when grounded
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // Jump
                if (_input.jump && _jumpTimeoutDelta <= 0.0f && PlayerValueManager.instance.currStamina - jumpStaminaReduceValue > 0)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
                    {
                        _verticalVelocity = Mathf.Sqrt(JumpHeightWithWeight * -2f * Gravity);
                    }
                    else
                    {
                        _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    }

                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }

                    PlayerValueManager.instance.RemoveStamina(jumpStaminaReduceValue);
                }

                // jump timeout
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // reset the jump timeout timer
                _jumpTimeoutDelta = JumpTimeout;

                // fall timeout
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // update animator if using character
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // if we are not grounded, do not jump
                _input.jump = false;
            }

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }

        public void CheckAttackClicks(int currAttackNumber)
        {
            if (attackClicks <= currAttackNumber)
            {
                attackClicks = 0;
                _animator.SetInteger("AttackClicks", attackClicks);

                if (!_animator.GetBool("HeavyAttack"))
                {
                    if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
                    {
                        _animator.speed = 1f;
                    }

                    if (_animator.GetLayerWeight(1) != 0)
                    {
                        _animator.SetLayerWeight(1, 0f);
                    }
                }
            }
        }

        public void ReduceStaminaWhenStartingAttack()
        {
            //if (!_animator.GetBool("Bow_Shoot"))
            //{
            //    _speed = 0;
            //    _animationBlend = 0;
            //    _animator.SetFloat(_animIDSpeed, 0);
            //}

            if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
            {
                _animator.speed = 0.6f;
            }

            if (_animator.GetBool("HeavyAttack") && attackClicks == 0)
            {
                PlayerValueManager.instance.RemoveStamina(heavyAttackStaminaReduceValue);
            }
            else
            {
                PlayerValueManager.instance.RemoveStamina(normalAttackStaminaReduceValue);
            }
        }       

        public void SetUsingStateToFalse()
        {
            _animator.SetBool("UsingHBItem", false);
            HotbarManager.instance.isUsingItem = false;
        }

        public void ResetBowShootingBool()
        {
            _animator.SetBool("Bow_Shoot", false);

            if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
            {
                _animator.speed = 1f;
            }
        }

        public void ResetHeavyAttackBool()
        {
            _animator.SetBool("HeavyAttack", false);

            if (InventoryManager.instance.currHoldingWeight > InventoryManager.instance.maxHoldingWeight || DebuffManager.instance.slowPlayerDebuff)
            {
                _animator.speed = 1f;
            }

            if (_animator.GetLayerWeight(1) != 0)
            {
                _animator.SetLayerWeight(1, 0f);
            }
        }

        public void HandleBowAimingCameras(CinemachineVirtualCamera newMainCam, CinemachineVirtualCamera newNotMainCam, CinemachineVirtualCamera newNotMainCam2)
        {
            //if (FightManager.instance.currTargetEnemy != null)
            //{
            //    if (newMainCam == _normalVCamera)
            //    {
            //        FightManager.instance.targetCVC.gameObject.SetActive(true);
            //    }
            //    else
            //    {
            //        FightManager.instance.targetCVC.gameObject.SetActive(false);
            //    }
            //}

            if (newMainCam != _normalVCamera)
            {
                FightManager.instance.crosshairGO.gameObject.SetActive(true);
            }
            else
            {
                FightManager.instance.crosshairGO.gameObject.SetActive(false);
            }

            newMainCam.Priority = 12;
            newNotMainCam.Priority = 11;
            newNotMainCam2.Priority = 10;
        }

        public void SetPlayerToSeatPos()
        {
            GameManager.instance.playerGO.transform.position = new Vector3(currSeatTrans.GetComponent<SeatingObject>().iOCanvasLookAtSitPlaceObj.transform.position.x, GameManager.instance.playerGO.transform.position.y,
                currSeatTrans.GetComponent<SeatingObject>().iOCanvasLookAtSitPlaceObj.transform.position.z);
        }

        public void SetAnimatorLayerBlending() // Only used in Animation Events
        {
            //if (_animator.GetLayerWeight(1) != 0.5f)
            //{
            //    _animator.SetLayerWeight(1, 0.5f);
            //}
            if (_animator.GetLayerWeight(1) != 0)
            {
                _animator.SetLayerWeight(1, 0f);
            }
        }
    }
}