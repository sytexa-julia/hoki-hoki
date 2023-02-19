/* ============================================================================================= = */
/* FMOD Ex - Error string header file. Copyright (c), Firelight Technologies Pty, Ltd. 2004-2005.  */
/*                                                                                                 */
/* Use this header if you want to store or display a string version / english explanation of       */
/* the FMOD error codes.                                                                           */
/*                                                                                                 */
/* =============================================================================================== */

namespace FMOD
{
    class Error
    {
        public static string String(FMOD.RESULT errcode)
        {
            switch (errcode)
            {
                case FMOD.RESULT.ERR_BADCOMMAND:          return "Tried to call a function on a data type that does not allow this type of functionality (ie calling Sound::lock / FMOD_Sound_Lock on a streaming sound).";
                case FMOD.RESULT.ERR_CDDA_DRIVERS:        return "Neither NTSCSI nor ASPI could be initialised.";
                case FMOD.RESULT.ERR_CDDA_INIT:           return "An error occurred while initialising the CDDA subsystem.";
                case FMOD.RESULT.ERR_CDDA_INVALID_DEVICE: return "Couldn't find the specified device.";
                case FMOD.RESULT.ERR_CDDA_NOAUDIO:        return "No audio tracks on the specified disc.";
                case FMOD.RESULT.ERR_CDDA_NODEVICES:      return "No CD/DVD devices were found.";
                case FMOD.RESULT.ERR_CDDA_NODISC:         return "No disc present in the specified drive.";
                case FMOD.RESULT.ERR_CDDA_READ:           return "A CDDA read error occurred.";
                case FMOD.RESULT.ERR_CHANNEL_ALLOC:       return "Error trying to allocate a channel.";
                case FMOD.RESULT.ERR_CHANNEL_STOLEN:      return "The specified channel has been reused to play another sound.";
                case FMOD.RESULT.ERR_COM:                 return "A Win32 COM related error occured. COM failed to initialize or a QueryInterface failed meaning a Windows codec or driver was not installed properly.";
                case FMOD.RESULT.ERR_DSP_CONNECTION:      return "DSP connection error.  Either the connection caused a cyclic dependancy or a generator unit attempted to have a unit attached to it.";
                case FMOD.RESULT.ERR_DSP_FORMAT:          return "DSP Format error.  A DSP unit may have attempted to connect to this network with the wrong format.  IE a floating point unit on a PocketPC system.";
                case FMOD.RESULT.ERR_DSP_NOTFOUND:        return "DSP connection error.  Couldn't find the DSP unit specified.";
                case FMOD.RESULT.ERR_DSP_RUNNING:         return "DSP error.  Cannot perform this operation while the network is in the middle of running.  This will most likely happen if a connection or disconnection is attempted in a DSP callback.";
                case FMOD.RESULT.ERR_FILE_BAD:            return "Error loading file.";
                case FMOD.RESULT.ERR_FILE_COULDNOTSEEK:   return "Couldn't perform seek operation.";
                case FMOD.RESULT.ERR_FILE_EOF:            return "End of file unexpectedly reached while trying to read essential data (truncated data?).";
                case FMOD.RESULT.ERR_FILE_NOTFOUND:       return "File not found.";
                case FMOD.RESULT.ERR_FORMAT:              return "Unsupported file or audio format.";
                case FMOD.RESULT.ERR_HTTP:                return "A HTTP error occurred. This is a catch-all for HTTP errors not listed elsewhere.";
                case FMOD.RESULT.ERR_HTTP_ACCESS:         return "The specified resource requires authentication or is forbidden.";
                case FMOD.RESULT.ERR_HTTP_PROXY_AUTH:     return "Proxy authentication is required to access the specified resource.";
                case FMOD.RESULT.ERR_HTTP_SERVER_ERROR:   return "A HTTP server error occurred.";
                case FMOD.RESULT.ERR_HTTP_TIMEOUT:        return "The HTTP request timed out.";
                case FMOD.RESULT.ERR_INITIALIZATION:      return "FMOD was not initialized correctly to support this function.";
                case FMOD.RESULT.ERR_INITIALIZED:         return "Cannot call this command after FMOD_System_Init.";
                case FMOD.RESULT.ERR_INTERNAL:            return "An error occured that wasnt supposed to.  Contact support.";
                case FMOD.RESULT.ERR_INVALID_HANDLE:      return "An invalid object handle was used.";
                case FMOD.RESULT.ERR_INVALID_PARAM:       return "An invalid parameter was passed to this function.";
                case FMOD.RESULT.ERR_MEMORY:              return "Not enough memory or resources.";
                case FMOD.RESULT.ERR_NET_CONNECT:         return "Couldn't connect to the specified host.";
                case FMOD.RESULT.ERR_NET_SOCKET_ERROR:    return "A socket error occurred.  This is a catch-all for socket-related errors not listed elsewhere.";
                case FMOD.RESULT.ERR_NET_URL:             return "The specified URL couldn't be resolved.";
                case FMOD.RESULT.ERR_NOTREADY:            return "Operation could not be performed because specified sound is not ready.";
                case FMOD.RESULT.ERR_OUTPUT_ALLOCATED:    return "Error initializing output device, but more specifically, the output device is already in use and cannot be reused.";
                case FMOD.RESULT.ERR_OUTPUT_CREATEBUFFER: return "Error creating hardware sound buffer.";
                case FMOD.RESULT.ERR_OUTPUT_DRIVERCALL:   return "A call to a standard soundcard driver failed, which could possibly mean a bug in the driver.";
                case FMOD.RESULT.ERR_OUTPUT_FORMAT:       return "Soundcard does not support the minimum features needed for this soundsystem (16bit stereo output).";
                case FMOD.RESULT.ERR_OUTPUT_INIT:         return "Error initializing output device.";
                case FMOD.RESULT.ERR_OUTPUT_NOHARDWARE:   return "FMOD_HARDWARE was specified but the sound card does not have the resources nescessary to play it.";
                case FMOD.RESULT.ERR_OUTPUT_NOSOFTWARE:   return "Attempted to create a software sound but no software channels were specified in System::init.";
                case FMOD.RESULT.ERR_PAN:                 return "Panning only works with mono or stereo sound sources.";
                case FMOD.RESULT.ERR_PLUGIN:              return "An unspecified error has been returned from a 3rd party plugin.";
                case FMOD.RESULT.ERR_PLUGIN_MISSING:      return "A requested output, dsp unit type or codec was not available.";
                case FMOD.RESULT.ERR_PLUGIN_RESOURCE:     return "A resource that the plugin requires cannot be found.";
                case FMOD.RESULT.ERR_RECORD:              return "An error occured trying to initialize the recording device.";
                case FMOD.RESULT.ERR_TAGNOTFOUND:         return "The specified tag could not be found or there are no tags.";
                case FMOD.RESULT.ERR_TOOMANYCHANNELS:     return "The sound created exceeds the allowable input channel count.  This can be increased with System::setMaxInputChannels";
                case FMOD.RESULT.ERR_UNIMPLEMENTED:       return "Something in FMOD hasn't been implemented when it should be! contact support!";
                case FMOD.RESULT.ERR_UNINITIALIZED:       return "This command failed because FMOD_System_Init or FMOD_System_SetDriver was not called.";
                case FMOD.RESULT.ERR_UNSUPPORTED:         return "A commmand issued was not supported by this object.  Possibly a plugin without certain callbacks specified.";
                case FMOD.RESULT.ERR_VERSION:             return "The version number of this file format is not supported.";
                case FMOD.RESULT.OK:                      return "No errors.";
                default :                                 return "Unknown error.";
            };
        }
    }
}
