using System;

namespace DealerOn.Core
{
    public class NASARemotelyControl
    {
        private int _moveRight = 0, _moveNorth = 0;
        private string _facingTo = "";

        /// <summary>
        /// set the exploration grid
        /// </summary>
        /// <param name="input">dimentions like 5 5, means to a grid 5x5</param>
        /// <returns>the exploration grid</returns>
        public String[,] SetGrid(string input)
        {
            int x = 0, y = 0;
            var dimensionGrid = input.Trim().Split(" ");
            var xIsNumber = int.TryParse(dimensionGrid[0], out x);
            var yIsNumber = int.TryParse(dimensionGrid[1], out y);
            if (dimensionGrid.Length != 2 || !xIsNumber || !yIsNumber)
            {
                throw new Exception("you aren't setting the space properly. It should be has 2 digits separated by space and must be numbers, example: \"3 3\"");
            }
            return new String[x + 1, y + 1];
        }

        /// <summary>
        /// do operation for calculating start point or movements that need to perform
        /// </summary>
        /// <param name="input">it can receive the starting point like 1 2 N and also can be the instructions for moving</param>
        /// <param name="explorationGrid">the grid where can operate those movements</param>
        /// <returns>return the last position and facing after performing instructions</returns>
        public string SetInstructions(string input, String[,] explorationGrid)
        {
            var RawCommand = input.Trim().Split(" ");
            if (RawCommand.Length == 3)
            {
                _moveRight = 0;
                _moveNorth = 0;
                _facingTo = "";
                var rIsNumber = int.TryParse(RawCommand[0], out _moveRight);
                var nIsNumber = int.TryParse(RawCommand[1], out _moveNorth);
                _facingTo = RawCommand[2].ToUpper();
                var fIsValidCompass = (
                    _facingTo == Instructions.N.ToString() ||
                    _facingTo == Instructions.S.ToString() ||
                    _facingTo == Instructions.W.ToString() ||
                    _facingTo == Instructions.E.ToString()) ? true : false;
                if (!rIsNumber || !nIsNumber || !fIsValidCompass)
                {
                    throw new Exception("Invalid Data, you should enter like \"1 2 E\"");
                }
                explorationGrid[_moveRight, _moveNorth] = _facingTo;
            }
            if (RawCommand.Length == 1)
            {
                var commands = input.Trim().ToCharArray();
                foreach (var command in commands)
                {
                    if ((char)Instructions.M == command)
                    {
                        (_moveRight, _moveNorth) = MoveForward(explorationGrid[_moveRight, _moveNorth], _moveRight, _moveNorth);
                        explorationGrid[_moveRight, _moveNorth] = _facingTo;
                    }
                    else if ((char)Instructions.L == command || (char)Instructions.R == command)
                    {
                        _facingTo = TurnLeftRight(_facingTo, command);
                        explorationGrid[_moveRight, _moveNorth] = _facingTo;
                    }
                }
            }
            return $"{_moveRight} {_moveNorth} {_facingTo}";
        }

        /// <summary>
        /// move left or right
        /// </summary>
        /// <param name="_facingTo">set the current facing</param>
        /// <param name="LR">if move left or right</param>
        /// <returns>return the orientations when move left or right</returns>
        private string TurnLeftRight(string _facingTo, char LR)
        {
            string result = "";
            Instructions instruction = (Instructions)System.Enum.Parse(typeof(Instructions), _facingTo);
            if (LR == (char)Instructions.R && instruction == Instructions.W)
                result = Instructions.N.ToString();
            else if (LR == (char)Instructions.L && instruction == Instructions.N)
                result = Instructions.W.ToString();
            else if (LR == (char)Instructions.R)
                result = (instruction + 1).ToString();
            else if (LR == (char)Instructions.L)
                result = (instruction - 1).ToString();
            return result;
        }
        /// <summary>
        /// move forward depends of its orientation
        /// </summary>
        /// <param name="orientation">the direction of its front</param>
        /// <param name="_moveRight">modify its position on X</param>
        /// <param name="_moveNorth">modify its position on Y</param>
        /// <returns>return the position after moving</returns>
        private (int moveR, int moveN) MoveForward(string orientation, int _moveRight, int _moveNorth)
        {
            Instructions instruction = (Instructions)System.Enum.Parse(typeof(Instructions), orientation);
            _moveRight += instruction == Instructions.E ? 1 : 0;
            _moveRight += instruction == Instructions.W ? -1 : 0;
            _moveNorth += instruction == Instructions.N ? 1 : 0;
            _moveNorth += instruction == Instructions.S ? -1 : 0;
            return (moveR: _moveRight, moveN: _moveNorth);
        }

        /// <summary>
        /// enum for operation with compass and other directions
        /// </summary>
        enum Instructions
        {
            N = 1,
            E = 2,
            S = 3,
            W = 4,

            L = 'L',
            R = 'R',
            M = 'M'
        }
    }
}
