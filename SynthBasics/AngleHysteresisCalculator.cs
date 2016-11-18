using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SynthBasics
{
    class AngleHysteresisCalculator
    {
        double _startingAngleInDegrees;
        bool _bWatchingZeroCrossing = true;
        bool _bMovingCCW = false;

        public void init(double startingAngleInDegrees)
        {
            _bWatchingZeroCrossing = true;
            _bMovingCCW = false;
            _startingAngleInDegrees = startingAngleInDegrees;
        }

        public double processAngle(double newAngleInDegrees)
        {
            double angledifference = newAngleInDegrees - _startingAngleInDegrees;

            //determine whether the angle difference is positive or negative by making sure the difference is -180 to 180
            if (angledifference > 180)
            {
                angledifference -= 360;
            }
            else if (angledifference < -180)
            {
                angledifference += 360;
            }

            if (_bWatchingZeroCrossing)
            {
                DetermineDirectionOfMovement(angledifference);
            }
            else //not watching for zero crossing -- see if we should be
            {
                DetectEntryIntoFirstQuadrant(angledifference);
            }

            //the calculation of the angle change depends on whether the motion is CW or CCW
            if (_bMovingCCW)
            {
                //allowable angles are 0 -> 360
                if (angledifference < 0)
                {
                    angledifference += 360;
                }
            }
            else
            {
                //allowable angles are 0 -> -360
                if (angledifference > 0)
                {
                    angledifference -= 360;
                }
            }

            return angledifference;
        }

        //assumes _bWatchingZeroCrossing is currently true
        private void DetermineDirectionOfMovement(double angledifference)
        {
            if (angledifference >= 0)
            {
                _bMovingCCW = true;
                if (angledifference > 90)
                {
                    _bWatchingZeroCrossing = false;
                }
            }
            else
            {
                _bMovingCCW = false;
                if (angledifference < -90)
                {
                    _bWatchingZeroCrossing = false;
                }
            }
        }

        //assumes _bWatchingZeroCrossing is currently false
        private void DetectEntryIntoFirstQuadrant(double angledifference)
        {
            if (_bMovingCCW && angledifference < 90 && angledifference >= 0)
            {
                _bWatchingZeroCrossing = true;
            }
            else
            {
                if (!_bMovingCCW && angledifference >= -90 && angledifference < 0)
                {
                    _bWatchingZeroCrossing = true;
                }
            }
        }
    }
}
