// Author: Jeremiah Kalmus
// File: compundC.cs
// Date: June 7th, 2019
// Version: 3.0

/* 
 * OVERVIEW:
 * compundC.cs during initialization will call the parent constructor for length p and the forbidden character (see pwdCheck.cs) and
 * also accept a third parameter which is a length k which determines how many times a compundC objects state can be toggled before
 * it loses the ability to toggle states. compundC objects act like pwdCheck objects except when a compundC object is on/active, a 
 * password will only be accepted if it satisfies the pwdCheck criteria, at least one character is repeated, and it cannot toggle 
 * states more than k times.
 * 
 * ---------------------------------------------------------------------------------------------------------------------------------
 * DESIGN DECISIONS AND ASSUMPTIONS:
 *  1 - If a state change parameter is entered and it is 0 or greater than 10, the default k value will be used which is 4. If no 
 *      parameters are entered, then deafult values will be set for p, k, and the forbidden character. The k value by default is 4,
 *      the p value and forbidden character are specified in pwdCheck.cs.
 *  
 *  2 - A Password repeating at least one character does not need to be successive characters. An example of acceptable passwords given
 *      their lengths are appropriate and no forbidden characters exist (e.g. ABCDEFGA). A is repeated and therefore passes that 
 *      constraint.
 *      
 *  3 - Accessors were provided to help the client keep track of the objects progress toward k state changes.
 *  
 *  4 - Working off of assumptions made in pwdCheck.cs.
 * ---------------------------------------------------------------------------------------------------------------------------------
 * INTERFACE INVARIANTS:
 *  1 - State Changes:
 *  
 *      State will change after p password checks where p is the minimum acceptable length for a password. After k state changes where
 *      k is the amount of state changes an object can have, the object will become locked and state change will no longer be possible.
 *  
 *  2 - Password_Check():
 *  
 *      Will check to see the state of the object and if it is on, then incoming passwords will be checked to see if any character
 *      repeats, as well as other functionality specified in Password_Check() in pwdCheck.cs.
 *  
 *  3 - Accessors (canToggle, State_Change_Limit):
 *  
 *      The client can view if the object can be toggled (meaning the object has not toggled k times yet) with canToggle. The client
 *      can also see the amount of times the object can toggle state with State_Change_Limit. All accessors from pwdCheck can also we
 *      called in compundC (see pwdCheck.cs).
 *      
 * ---------------------------------------------------------------------------------------------------------------------------------
 * IMPLEMENTATION INVARIANTS:
 *  1 - Password_Check:
 *      
 *      This method is overridden since there was need for additional password checks in addition to those that exist in pwdCheck.cs.
 *      
 *  2 - Toggle_State:
 *  
 *      This method is overridden since it needed to be modified to keep track of how many state changes there have been and to lock out
 *      the objects toggling capabilities after k toggles.
 *      
 *  3 - See pwdCheck.cs for more information.
 *  
 * ---------------------------------------------------------------------------------------------------------------------------------
 * CLASS INVARIANTS:
 *  1 - CONSTRUCTOR:
 *  
 *      Takes in 3 parameters, one of which being the amount of times a compundC object can toggle states (value k). Initializes data
 *      members pertaining specifically to the compundC class. Parent constructor also fired. See pwdCheck.cs.
 *      
 *  2 - On/Active State:
 *  
 *      Same as the pwdCheck.cs with the addition of checking incoming passwords for at least one repeating character. Also, checking
 *      the number of state toggles the object has done and preventing more than k state toggles.
 *      
 *  3 - Off/Inactive State:
 *  
 *      Refer to pwdCheck.cs Off/Inactive state.
 * ---------------------------------------------------------------------------------------------------------------------------------
 */

using System;

namespace p6
{
    class compundC : pwdCheck
    { 

        private const uint DEFAULT_STATE_CHANGES = 4;
        private const uint MAX_STATE_CHANGES = 10;
        private readonly uint state_change_limit;
        private uint state_change_counter;
        private bool can_toggle;

        public compundC(uint max_state_changes = DEFAULT_STATE_CHANGES, uint default_password_length = DEFAULT_PASSWORD_LENGTH,
                       char forbidden_char_input = ' ') : base(default_password_length, forbidden_char_input)
        {
            if (max_state_changes > MAX_STATE_CHANGES || max_state_changes == 0)
            {
                state_change_limit = DEFAULT_STATE_CHANGES;
            }
            else
            {
                state_change_limit = max_state_changes;
            }
            state_change_counter = 0;
            can_toggle = true;
        }
        public bool canToggle
        {
            get
            {
                return can_toggle;
            }
        }
        public uint State_Change_Limit
        {
            get
            {
                return state_change_limit;
            }
        }
        // PRE: Client must enter a password.
        // POST: State may be toggled, state_counter may be incremented, and the ability to toggle may be switched
        //       from true to false.
        public override bool Password_Check(string input_password)
        {
            actual_password_length = (uint)input_password.Length;

            if (Max_Toggle_Check())
            {
                if (state_counter == minimum_password_length)
                {
                    Toggle_State();
                }
            }

            if (!Repeat_Character_Check(input_password))
            {
                return false;
            }
            return base.Password_Check(input_password);
        }
        // PRE: Should only be called when state_counter is equal to p.
        // POST: May change state of object and increment state_change_counter. 
        protected override bool Toggle_State()
        {
            if (can_toggle)
            {
                state_change_counter++;
                return base.Toggle_State();
            }
            return false;
        }
        // PRE: State of object must be active.
        private bool Repeat_Character_Check(string input_password)
        {
            for (int i = 1; i < actual_password_length; i++)
            {
                int j = 0;
                while (j < i)
                {
                    if (input_password[i] == input_password[j])
                    {
                        return true;
                    }
                    j++;
                }
            }
            return false;
        }
        // POST: May change the objects ability to toggle state.
        private bool Max_Toggle_Check()
        {
            if (state_change_counter == state_change_limit)
            {
                can_toggle = false;
            }
            return can_toggle;
        }

    }
}
