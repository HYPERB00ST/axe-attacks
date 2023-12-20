namespace NPCLogic {
    partial class StateInfo {
        public enum StatesId {
            Idle,
            Chase,
            Combat,
            Return,
            Dead
        }
        
        // Constants
        internal const float CHASE_RANGE = 5f;
        internal const float COMBAT_RANGE = 2f;
        internal const float RETURN_RANGE = 10f;
        internal const int NO_CHANGE_OF_STATE = -1;
    }
}