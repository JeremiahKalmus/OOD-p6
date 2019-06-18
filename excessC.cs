// Author: Jeremiah Kalmus
// File: excessC.cs
// Date: June 7th, 2019
// Version: 3.0

/* 
 * OVERVIEW:
 * excessC.cs during construction will call the parent pwdCheck constructor for length p and the forbidden character (see pwdCheck.cs).
 * excessC in its off state acts like pwdCheck in its on state. In excessC's on state, it checks if $ is in the password, if there is 
 * at least one lower case and one upper case character in the password, and that the pth character in the password is a digit.
 * ---------------------------------------------------------------------------------------------------------------------------------
 * DESIGN DECISIONS AND ASSUMPTIONS:
 *  1 - Starts in off state where excessC acts like pwdCheck when pwdCheck is in its on state.
 *  
 *  2 - excessC's on state only checks if $ is in the password, if there is at least one lower case and one upper case character in 
 *      the password, and that the pth character in the password is a digit. It does not have any functionality from pwdChck.
 * ---------------------------------------------------------------------------------------------------------------------------------
 * INTERFACE INVARIANTS:
 *  1 - State Changes:
 *  
 *      State will change after p password checks where p is the minimum acceptable length for a password. 
 *  
 *  2 - Password_Check():
 *  
 *      Will check to see the state of the object and if it is on, then incoming passwords will be checked to see if there is at least
 *      one upper case and one lower case letter in the password, $ is in the password, and that the pth character in the password be a
 *      digit. If the object is in its off state, it will act just like pwdCheck (see pwdCheck.cs inteface invariants).
 *  
 *  3 - Accessors (isActive):
 *  
 *      The client can use isActive to keep track of whether the excessC object is on/active or off/inactive.
 *  
 * ---------------------------------------------------------------------------------------------------------------------------------
 * IMPLEMENTATION INVARIANTS:
 *  1 - Password_Check:
 *      
 *      This method is overridden since there was need for additional password checks in addition to those that exist in pwdCheck.cs.
 *      
 *  2 - Toggle_State:
 *  
 *      This method is overridden since it needs to constantly keep its own state opposite that of pwdCheck. If pwdCheck state is on
 *      then excessC is in its off state and vice-versa.
 *      
 *  3 - isActive:
 *  
 *      This accessor is overridden since excessC uses pwdCheck state to keep track of its own state, however, the states are inversed.
 *      This means that is pwdCheck object shows on/active, then excessC is really off/inactive.
 *      
 *  4 - See pwdCheck.cs for more information.
 *  
 * ---------------------------------------------------------------------------------------------------------------------------------
 * CLASS INVARIANTS:
 *  1 - CONSTRUCTOR:
 *  
 *      Initialized private data members and fires pwdCheck constructor. See CONSTRUCTOR in pwdCheck.cs for further information.
 *      
 *  2 - On/Active State:
 *  
 *      Checks incoming passwords to see if $ is in the password, if there is at least one lower case and one upper case character in 
 *      the password, and that the pth character in the password is a digit. It does not have any functionality from pwdChck.
 *  
 *  3 - Off/Inactive State:
 *  
 *      Acts exactly like pwdCheck when pwdCheck is in an on state.
 *      
 * ---------------------------------------------------------------------------------------------------------------------------------
 */
 /* UPDATES:
  * 
  * 6/5/2019 - Removed the data member 'on'. Relying on the parent '!active' for state.
  *            From this, excessC has no need to override a toggle_state method, therefore,
  *            it will use the parents toggle_state method. 
  * 
  * 
  */

using System;

namespace p6
{
    class excessC : pwdCheck
    {
        private const char DOLLAR_SIGN = '$';
        public excessC(uint default_password_length = DEFAULT_PASSWORD_LENGTH, char forbidden_char_input = ' ') 
            : base(default_password_length, forbidden_char_input)
        {}
        override public bool isActive
        {
            get
            {
                return !active;
            }
        }
        // PRE: Password needs to be entered by the client
        // POST: Object state may be toggled and state_counter may be incremented
        public override bool Password_Check(string input_password)
        {
            if (active)
            {
                return base.Password_Check(input_password);
            }
            else
            {
                actual_password_length = (uint)input_password.Length;

                if (state_counter == minimum_password_length)
                {
                    Toggle_State();
                }

                if (Password_Length_Check(input_password))
                {
                    if (Character_P_isDigit_Check(input_password) && Dollar_Sign_Check(input_password) && 
                        Mixed_Case_Check(input_password))
                    {
                        state_counter++;
                        return true;
                    }
                }
                state_counter++;
                return false;
            }
        }
        // PRE: State of object must be active.
        private bool Character_P_isDigit_Check(string input_password)
        {
            return (Char.IsDigit(input_password[(int)minimum_password_length - 1]));
        }
        // PRE: State of object must be active.
        private bool Dollar_Sign_Check(string input_password)
        {
            for (int i = 0; i < actual_password_length; i++)
            {
                if (input_password[i] == DOLLAR_SIGN)
                {
                    return true;
                }
            }
            return false;
        }
        // PRE: State of object must be active.
        private bool Mixed_Case_Check(string input_password)
        {
            uint isUpper_counter = 0;
            uint isLower_counter = 0;

            for (int i = 0; i < actual_password_length; i++)
            {
                if (Char.IsUpper(input_password[i]))
                {
                    isUpper_counter++;
                }
                else if (Char.IsLower(input_password[i]))
                {
                    isLower_counter++;
                }
            }
            return (isUpper_counter != 0 && isLower_counter != 0);
        }
    }
}
