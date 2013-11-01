using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USBMissileLanch
{
    public interface ILauncherDevice
    {
        /// <summary>
        /// Name of the device (human readable)
        /// </summary>
        string ModelName { get; }

        /// <summary>
        /// HID Usage ID
        /// </summary>
        ushort UsageID { get; }

        /// <summary>
        /// HID Usage Page
        /// </summary>
        ushort UsagePage { get; }

        /// <summary>
        /// VendorId of the device
        /// </summary>
        ushort VendorId { get; }

        /// <summary>
        /// DeviceId of the device
        /// </summary>
        ushort DeviceId { get; }

        /// <summary>
        /// Command to move down
        /// </summary>
        byte[] Down { get; }

        /// <summary>
        /// Command to move up
        /// </summary>
        byte[] Up { get; }

        /// <summary>
        /// Command to turn left
        /// </summary>
        byte[] Left { get; }

        /// <summary>
        /// Command to turn right
        /// </summary>
        byte[] Right { get; }

        /// <summary>
        /// Command to stop moving
        /// </summary>
        byte[] Stop { get; }

        /// <summary>
        /// Command to fire a missile
        /// </summary>
        byte[] Fire { get; }

        /// <summary>
        /// Minimum number of shots possible (usually 1)
        /// </summary>
        int MinNumberOfShots { get; }

        /// <summary>
        /// Maximum number of shots possible
        /// </summary>
        int MaxNumberOfShots { get; }

        /// <summary>
        /// Reset position: move X milliseconds left
        /// </summary>
        int ResetTimeLeft { get; }

        /// <summary>
        /// Reset position: move X milliseconds down
        /// </summary>
        int ResetTimeDown { get; }

        /// <summary>
        /// Wait X milliseconds before first shot
        /// </summary>
        int WaitBeforeFire { get; }

        /// <summary>
        /// Wait X milliseconds after each shot
        /// </summary>
        int WaitAfterFire { get; }

        /// <summary>
        /// Create the final command to send to the launcher
        /// </summary>
        /// <param name="command">The command to send (move, fire etc.)</param>
        /// <returns>byte array to send to the launcher</returns>
        byte[] CreateCommand(byte[] command);
 
    }
}
