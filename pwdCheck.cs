// Author: Jeremiah Kalmus
// File: pwdCheck.cs
// Date: June 7th, 2019
// Version: 3.0

/* 
 * OVERVIEW:
 *  pwdCheck.cs during construction takes 2 parameter values and defines a minimum length requirement for passwords and a
 *  defult forbidden character. The client can then enter a password and pwdCheck will either accept the password or reject it
 *  depending if it meets the criteria of being a certain length p, if it contains no traces of a forbidden character, and if it
 *  is composed completely of characters between ASCII 32 and 127.
 * ---------------------------------------------------------------------------------------------------------------------------------
 * DESIGN DECISIONS AND ASSUMPTIONS:
 *  1 - ASCII characters less than 32 are rejected since they are considered control characters.
 *  
 *  2 - ASCII character greater than 127 are rejected since they are non-frequently used special characters or accented characters
 *      that do not pertain to the english language.
 *      
 *  3 - Passwords cannot be negative length or length less than 4 or length greater than 30 since a password of negative length doesn't
 *      exist, a password length of less than 4 is an insecure password, and a password length of over 30 is unneccessary. Also, the 
 *      length of p follows the same rules except the maximum it can be is 20.
 *      
 *  4 - If the the a pwdCheck object is off/inactive, then all passwords will be rejected.
 *      
 * ---------------------------------------------------------------------------------------------------------------------------------
 * INTERFACE INVARIANTS:
 *  1 - State Changes:
 *  
 *      A pwdCheck object changes state if p number of passwords have been checked, where p is the minimum length a password is required
 *      to be in order to be accepted. 
 *      
 *  2 - Password_Check():
 *  
 *      When a pwdCheck object is on/active, it checks entered passwords into Password_Check() to see if they are at least length p,
 *      that each character is an ASCII character between 32 and 127, and that a forbidden character does not exist in the password.
 *      The method will return true if all these conditions are met and false otherwise. A pwdCheck object will reject all passwords
 *      if it is in an off/inactive state.
 *  
 *  3 - Accessors (Password_Length, Minimum_Password_Length, isActive):
 *  
 *      The client can request access to see the length of the last entered password with Password_Length, the minimum length 
 *      requirement for the password to be accepted (p length) with Minimum_Password_Length, and the status of the object with 
 *      isActive.
 *  
 * ---------------------------------------------------------------------------------------------------------------------------------
 * IMPLEMENTATION INVARIANTS:
 *  1 - Protected is used for all data members since it is known that other classes will rely on pwdCheck as a parent class and will 
 *      need to access the data members of the parent for optimal use.
 *      
 *  2 - isActive, Password_Check, and Toggle_State are all defined as virtual since child classes may need to override and modify the
 *      functionality of these pwdCheck methods. 
 *      
 *  3 - State is toggled every p passwords that are entered.
 *  
 *  4 - Minimum password length by dafault is 6.
 *  
 *  5 - Default forbidden characrer is a space.
 *  
 *  6 - State is toggled in the Toggle_State() potected method. Data member keeping track of state it called 'active'.
 *  
 * ---------------------------------------------------------------------------------------------------------------------------------
 * CLASS INVARIANTS:
 *  1 - CONSTRUCTOR:
 *  
 *      Client can input up to 2 parameters. The parameters are minimum required password length (p), and a forbidden character. If the
 *      client enters fewer paremeters, then the parameters that are not filled will be set to a default value. Default length p is
 *      '6' and the default forbidden character is a 'space'. The constructor will also check to see if the forbidden character is an
 *      ASCII character between 32 and 127. If the forbidden character is not within these bounds, then it will be set to its default 
 *      value. Also, if the length of p entered is less than 4 or greater than 20, the p value will be set to its default. In the end,
 *      the constructor initializes all data members.
 *      
 *  2 - On/Active State:
 *  
 *      Checks passwords entered by the client to see if they are at least length p, whether all characters of the password are ASCII
 *      characters between ASCII 32 and ASCII 127, and if the forbidden character doesn't exist in the password. The state of a pwdCheck
 *      object will toggle between on and off if p passwords are checked.
 *  
 *  3 - Off/Inactive State: 
 *      
 *      Rejects all passwords.
 *      
 *  4 - IPwdCheck Interface:
 *  
 *      Provides all public functionality to whichever class desires the pwdCheck public functionality
 *      
 * ---------------------------------------------------------------------------------------------------------------------------------
 */
 /*
  * UPDATES:
  * 
  * 6/6/2019 - Added a IPwdCheck interface that includes all of pwdCheck public functionality to the use of supporting
  *            simulated multiple inheritance in c#.
  */

using System;

namespace p6
{
    interface IpwdCheck
    {
        bool Password_Check(string input_password);
        uint Password_Length
        {
            get;
        }
        uint Minimum_Password_Length
        {
            get;
        }
        bool isActive
        {
            get;
        }
    }
    class pwdCheck : IpwdCheck
    {
        protected const uint DEFAULT_PASSWORD_LENGTH = 6;
        protected const uint MAX_PWD_LENGTH_REQUIRMENT = 20;
        protected const uint MIN_PASSWORD_LENGTH = 4;
        protected const uint ASCII_MIN = 32;
        protected const uint ASCII_MAX = 127;
        protected const uint MAX_PWD_LENGTH = 30;
        protected const char DEFAULT_FROBIDDEN_CHAR = ' ';
        protected readonly uint minimum_password_length;
        protected bool active;
        protected uint state_counter;
        protected uint actual_password_length;
        protected char forbidden_character;
        public pwdCheck(uint default_password_length = DEFAULT_PASSWORD_LENGTH, char forbidden_char_input = ' ')
        {
            if (default_password_length < MIN_PASSWORD_LENGTH || default_password_length > MAX_PWD_LENGTH_REQUIRMENT)
            {
                minimum_password_length = DEFAULT_PASSWORD_LENGTH;
            }
            else
            {
                minimum_password_length = default_password_length;
            }
            if (forbidden_char_input < ASCII_MIN || forbidden_char_input > ASCII_MAX)
            {
                forbidden_character = DEFAULT_FROBIDDEN_CHAR;
            }
            active = true;
            state_counter = 0;
            actual_password_length = 0;
            forbidden_character = forbidden_char_input;
        }
        public uint Password_Length
        {
            get
            {
                return actual_password_length;
            }
        }
        public uint Minimum_Password_Length
        {
            get
            {
                return minimum_password_length;
            }
        }
        virtual public bool isActive
        {
            get
            {
                return active;
            }
        }
        // PRE: Password needs to be entered by the client
        // POST: Object state may be toggled and state_counter may be incremented
        virtual public bool Password_Check(string input_password)
        {
            if (state_counter == minimum_password_length)
            {
                Toggle_State();
            }

            actual_password_length = (uint)input_password.Length;

            if (active)
            {
                if (Forbidden_Char_And_ASCII_Check(input_password) && Password_Length_Check(input_password))
                {
                    state_counter++;
                    return true;
                }
            }
            state_counter++;
            return false;
        }
        // PRE: State of object must be active
        protected bool Password_Length_Check(string input_password)
        {
            return (actual_password_length >= minimum_password_length && actual_password_length <= MAX_PWD_LENGTH);
        }
        // PRE: Should only be called when state_counter is equal to p.
        // POST: State of object will be toggled and state_counter will be reset.
        virtual protected bool Toggle_State()
        {
            state_counter = 0;
            return active = !active;
        }
        // PRE: State of object must be active
        private bool Forbidden_Char_And_ASCII_Check(string input_password)
        {
            for (int i = 0; i < actual_password_length; i++)
            {
                if (input_password[i] == forbidden_character || ((input_password[i] < ASCII_MIN) ||
                    (input_password[i] > ASCII_MAX)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
