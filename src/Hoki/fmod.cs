/* ========================================================================================== */
/* FMOD Ex - C# Wrapper . Copyright (c), Firelight Technologies Pty, Ltd. 2004-2005.          */
/*                                                                                            */
/*                                                                                            */
/* ========================================================================================== */

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace FMOD
{
    /*
        FMOD version number.  Check this against FMOD::System::getVersion / System_GetVersion
        0xaaaabbcc -> aaaa = major version number.  bb = minor version number.  cc = development version number.
    */
    public class VERSION
    {
        public const int    number = 0x00040028;
        public const string dll    = "fmodex.dll";
    }

    /*
        FMOD types 
    */
    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]   
        Structure describing a point in 3D space.

        [REMARKS]
        FMOD uses a left handed co-ordinate system by default.<br>
        To use a right handed co-ordinate system specify FMOD_INIT_3D_RIGHTHANDED from FMOD_INITFLAGS in System::init.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        System::set3DListenerAttributes
        System::get3DListenerAttributes
        Channel::set3DAttributes
        Channel::get3DAttributes
        Geometry::addPolygon
        Geometry::setPolygonVertex
        Geometry::getPolygonVertex
        Geometry::setRotation
        Geometry::getRotation
        Geometry::setPosition
        Geometry::getPosition
        Geometry::setScale
        Geometry::getScale
        FMOD_INITFLAGS
    ]
    */
    public struct VECTOR
    {
        public float x;        /* X co-ordinate in 3D space. */
        public float y;        /* Y co-ordinate in 3D space. */
        public float z;        /* Z co-ordinate in 3D space. */
    }

    /*
    [ENUM]
    [
        [DESCRIPTION]   
        error codes.  Returned from every function.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
    ]
    */
    public enum RESULT
    {
        OK,                             /* No errors. */
        ERR_BADCOMMAND,                 /* Tried to call a function on a data type that does not allow this type of functionality (ie calling Sound::lock / Sound_Lock on a streaming sound). */
        ERR_CDDA_DRIVERS,               /* Neither NTSCSI nor ASPI could be initialised. */
        ERR_CDDA_INIT,                  /* An error occurred while initialising the CDDA subsystem. */
        ERR_CDDA_INVALID_DEVICE,        /* Couldn't find the specified device. */
        ERR_CDDA_NOAUDIO,               /* No audio tracks on the specified disc. */
        ERR_CDDA_NODEVICES,             /* No CD/DVD devices were found. */ 
        ERR_CDDA_NODISC,                /* No disc present in the specified drive. */
        ERR_CDDA_READ,                  /* A CDDA read error occurred. */
        ERR_CHANNEL_ALLOC,              /* Error trying to allocate a channel. */
        ERR_CHANNEL_STOLEN,             /* The specified channel has been reused to play another sound. */
        ERR_COM,                        /* A Win32 COM related error occured. COM failed to initialize or a QueryInterface failed meaning a Windows codec or driver was not installed properly. */
        ERR_DSP_CONNECTION,             /* DSP connection error.  Either the connection caused a cyclic dependancy or the unit issues variable sized reads and tried to connect to a node that was already connected to. */
        ERR_DSP_FORMAT,                 /* DSP Format error.  A DSP unit may have attempted to connect to this network with the wrong format.  IE a floating point unit on a PocketPC system. */
        ERR_DSP_NOTFOUND,               /* DSP connection error.  Couldn't find the DSP unit specified. */
        ERR_DSP_RUNNING,                /* DSP error.  Cannot perform this operation while the network is in the middle of running.  This will most likely happen if a connection or disconnection is attempted in a DSP callback. */
        ERR_DSP_TOOMANYCONNECTIONS,     /* DSP connection error.  The unit being connected to or disconnected should only have 1 input or output. */
        ERR_EAX4_INSTANCE,              /* Specified Instance in FMOD_REVERB_PROPERTIES couldn't be set. Most likely because another application has locked the EAX4 FX slot.*/
        ERR_FILE_BAD,                   /* Error loading file. */
        ERR_FILE_COULDNOTSEEK,          /* Couldn't perform seek operation.  This is a limitation of the medium (ie netstreams) or the file format. */
        ERR_FILE_EOF,                   /* End of file unexpectedly reached while trying to read essential data (truncated data?). */
        ERR_FILE_NOTFOUND,              /* File not found. */
        ERR_FORMAT,                     /* Unsupported file or audio format. */
        ERR_HTTP,                       /* A HTTP error occurred. This is a catch-all for HTTP errors not listed elsewhere. */
        ERR_HTTP_ACCESS,                /* The specified resource requires authentication or is forbidden. */
        ERR_HTTP_PROXY_AUTH,            /* Proxy authentication is required to access the specified resource. */
        ERR_HTTP_SERVER_ERROR,          /* A HTTP server error occurred. */
        ERR_HTTP_TIMEOUT,               /* The HTTP request timed out. */
        ERR_INITIALIZED,                /* Cannot call this command after System_Init. */
        ERR_INITIALIZATION,             /* FMOD was not initialized correctly to support this function. */
        ERR_INTERNAL,                   /* An error occured that wasnt supposed to.  Contact support. */
        ERR_INVALID_HANDLE,             /* An invalid object handle was used. */
        ERR_INVALID_PARAM,              /* An invalid parameter was passed to this function. */
        ERR_MEMORY,                     /* Not enough memory or resources. */
        ERR_NEEDSOFTWARE,               /* Tried to use a feature that requires the software engine but the software engine has been turned off. */
        ERR_NET_CONNECT,                /* Couldn't connect to the specified host. */
        ERR_NET_SOCKET_ERROR,           /* A socket error occurred.  This is a catch-all for socket-related errors not listed elsewhere. */
        ERR_NET_URL,                    /* The specified URL couldn't be resolved. */
        ERR_NOTREADY,                   /* Operation could not be performed because specified sound is not ready. */
        ERR_OUTPUT_ALLOCATED,           /* Error initializing output device, but more specifically, the output device is already in use and cannot be reused. */
        ERR_OUTPUT_CREATEBUFFER,        /* Error creating hardware sound buffer. */
        ERR_OUTPUT_DRIVERCALL,          /* A call to a standard soundcard driver failed, which could possibly mean a bug in the driver or resources were missing or exhausted. */
        ERR_OUTPUT_FORMAT,              /* Soundcard does not support the minimum features needed for this soundsystem (16bit stereo output). */
        ERR_OUTPUT_INIT,                /* Error initializing output device. */
        ERR_OUTPUT_NOHARDWARE,          /* HARDWARE was specified but the sound card does not have the resources nescessary to play it. */
        ERR_OUTPUT_NOSOFTWARE,          /* Attempted to create a software sound but no software channels were specified in System::init. */
        ERR_PAN,                        /* Panning only works with mono or stereo sound sources. */
        ERR_PLUGIN,                     /* An unspecified error has been returned from a 3rd party plugin. */
        ERR_PLUGIN_MISSING,             /* A requested output, dsp unit type or codec was not available. */
        ERR_PLUGIN_RESOURCE,            /* A resource that the plugin requires cannot be found. */
        ERR_RECORD,                     /* An error occured trying to initialize the recording device. */
        ERR_TAGNOTFOUND,                /* The specified tag could not be found or there are no tags. */
        ERR_TOOMANYCHANNELS,            /* The sound created exceeds the allowable input channel count.  This can be increased with System::setMaxInputChannels */
        ERR_UNIMPLEMENTED,              /* Something in FMOD hasn't been implemented when it should be! contact support! */
        ERR_UNINITIALIZED,              /* This command failed because System_Init or System_SetDriver was not called. */
        ERR_UNSUPPORTED,                /* A commmand issued was not supported by this object.  Possibly a plugin without certain callbacks specified. */
        ERR_VERSION                     /* The version number of this file format is not supported. */
    }



    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These output types are used with System::setOutput/System::getOutput/FMOD_System_SetOutput/FMOD_System_GetOutput, to choose which output method to use.
  
        [REMARKS]
        To drive the output synchronously, and to disable FMOD's timing thread, use the INIT_NONREALTIME flag.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        System::setOutput
        System::getOutput
        System::setOutputFormat
        System::getOutputFormat
    ]
    */
    public enum OUTPUTTYPE
    {
        AUTODETECT,   /* Picks the best output mode for the platform.  This is the default. */

        UNKNOWN,      /* All         - 3rd party plugin, unknown.  This is for use with System::getOutput only. */
        NOSOUND,      /* All         - All calls in this mode succeed but make no sound. */
        WAVWRITER,    /* All         - Writes output to fmodout.wav by default.  Use System::setOutputFormat to set the filename. */

        DSOUND,       /* Win32/Win64 - DirectSound output.  Use this to get EAX Reverb support. */
        WINMM,        /* Win32/Win64 - Windows Multimedia output. */
        ASIO,         /* Win32       - Low latency ASIO driver. */
        OSS,          /* Linux       - Open Sound System output. */
        ALSA,         /* Linux       - Advanced Linux Sound Architecture output. */
        ESD,          /* Linux       - Enlightment Sound Daemon output. */
        SOUNDMANAGER, /* Mac         - Macintosh SoundManager output. */
        COREAUDIO,    /* Mac         - Macintosh CoreAudio output */
        XBOX,         /* Xbox        - Native hardware output. */
        PS2,          /* PS2         - Native hardware output. */
        GC,           /* GameCube    - Native hardware output. */
        XENON,        /* Xbox 2      - Native hardware output. */
        PSP,          /* PSP         - Native hardware output. */
    }


    /*
    [ENUM] 
    [
        [DESCRIPTION]   

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
    ]
    */
    public enum CAPS
    {
        NONE                   = 0x00000000,    /* Device has no special capabilities. */
        HARDWARE               = 0x00000001,    /* Device supports hardware mixing. */
        HARDWARE_EMULATED      = 0x00000002,    /* Device supports 'hardware' mixing but it will be mixed on the CPU by the driver. */
        OUTPUT_MULTICHANNEL    = 0x00000004,    /* Device can do multichannel output, ie greater than 2 channels. */
        OUTPUT_FORMAT_PCM8     = 0x00000008,    /* Device can output to 8bit integer PCM. */
        OUTPUT_FORMAT_PCM16    = 0x00000010,    /* Device can output to 16bit integer PCM. */
        OUTPUT_FORMAT_PCM24    = 0x00000020,    /* Device can output to 24bit integer PCM. */
        OUTPUT_FORMAT_PCM32    = 0x00000040,    /* Device can output to 32bit integer PCM. */
        OUTPUT_FORMAT_PCMFLOAT = 0x00000080,    /* Device can output to 32bit floating point PCM. */
        REVERB_EAX2            = 0x00000100,    /* Device supports EAX2 reverb. */
        REVERB_EAX3            = 0x00000200,    /* Device supports EAX3 reverb. */
        REVERB_EAX4            = 0x00000400,    /* Device supports EAX4 reverb  */
        REVERB_I3DL2           = 0x00000800,    /* Device supports I3DL2 reverb. */
        REVERB_LIMITED         = 0x00001000     /* Device supports some form of limited hardware reverb, maybe parameterless and only selectable by environment. */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These are speaker types defined for use with the System::setSpeakerMode / System_SetSpeakerMode or 
        System::getSpeakerMode / System_GetSpeakerMode command.

        [REMARKS]
        These are important notes on speaker modes in regards to sounds created with SOFTWARE.<br>
        Note below the phrase 'sound channels' is used.  These are the subchannels inside a sound, they are not related and 
        have nothing to do with the FMOD class "Channel / CHANNEL".<br>
        For example a mono sound has 1 sound channel, a stereo sound has 2 sound channels, and an AC3 or 6 channel wav file have 6 "sound channels".<br>
        <br>
        FMOD_SPEAKERMODE_RAW<br>
        ---------------------<br>
        This mode is for output devices that are not specifically mono/streo/5.1 or 7.1, but are multichannel.<br>
        Sound channels map to speakers sequentially, so a mono sound maps to output speaker 0, stereo sound maps to output speaker 0 & 1.<br>
        Multichannel sounds map input channels to output channels 1:1. <br>
        Channel::setPan / Channel_SetPan and Channel::setSpeakerMix / Channel_SetSpeakerMix do not work.<br>
        Speaker levels must be manually set with Channel::setSpeakerLevels / Channel_SetSpeakerLevels.<br>
        <br>
        SPEAKERMODE_MONO<br>
        ---------------------<br>
        This mode is for a 1 speaker arrangement.<br>
        Panning does not work in this speaker mode.<br>
        Mono, stereo and multichannel sounds have each sound channel played on the one speaker unity.<br>
        Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels / Channel_SetSpeakerLevels.<br>
        Channel::setSpeakerMix / Channel_SetSpeakerMix does not work.<br>
        <br>
        SPEAKERMODE_STEREO<br>
        -----------------------<br>
        This mode is for 2 speaker arrangements that have a left and right speaker.<br>
        Mono sounds default to an even distribution between left and right.  They can be panned with Channel::setPan / Channel_SetPan.<br>
        Stereo sounds default to the middle, or full left in the left speaker and full right in the right speaker.  
        They can be cross faded with Channel::setPan / Channel_SetPan.<br>
        Multichannel sounds have each sound channel played on each speaker at unity.<br>
        Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels / Channel_SetSpeakerLevels.<br>
        Channel::setSpeakerMix / Channel_SetSpeakerMix works but only left and right parameters are used, the rest are ignored.<br>
        <br>
        FMOD_SPEAKERMODE_4POINT1<br>
        ------------------------<br>
        This mode is for 4.1 speaker arrangements that have a left/right/center/rear and a subwoofer speaker.<br>
        Mono sounds default to the center speaker.  They can be panned with Channel::setPan.<br>
        Stereo sounds default to the left sound channel played on the front left, and the right sound channel played on the front right.  
        They can be cross faded with Channel::setPan.<br>
        Multichannel sounds default to all of their sound channels being played on each speaker in order of input.
        Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels.<br>
        Channel::setSpeakerMix works but side left / side right are ignored, and rear left / rear right are averaged.<br>
        <br>
        SPEAKERMODE_5POINT1<br>
        ------------------------<br>
        This mode is for 5.1 speaker arrangements that have a left/right/center/rear left/rear right and a subwoofer speaker.<br>
        Mono sounds default to the center speaker.  They can be panned with Channel::setPan / Channel_SetPan.<br>
        Stereo sounds default to the left sound channel played on the front left, and the right sound channel played on the front right.  
        They can be cross faded with Channel::setPan / Channel_SetPan.<br>
        Multichannel sounds default to all of their sound channels being played on each speaker in order of input.  
        Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels / Channel_SetSpeakerLevels.<br>
        Channel::setSpeakerMix / Channel_SetSpeakerMix works but side left / side right are ignored.<br>
        <br>
        SPEAKERMODE_7POINT1<br>
        ------------------------<br>
        This mode is for 7.1 speaker arrangements that have a left/right/center/rear left/rear right/side left/side right 
        and a subwoofer speaker.<br>
        Mono sounds default to the center speaker.  They can be panned with Channel::setPan / Channel_SetPan.<br>
        Stereo sounds default to the left sound channel played on the front left, and the right sound channel played on the front right.  
        They can be cross faded with Channel::setPan / Channel_SetPan.<br>
        Multichannel sounds default to all of their sound channels being played on each speaker in order of input.  
        Mix behaviour for multichannel sounds can be set with Channel::setSpeakerLevels / Channel_SetSpeakerLevels.<br>
        Channel::setSpeakerMix / Channel_SetSpeakerMix works and every parameter is used to set the balance of a sound in any speaker.<br>
        <br>
        SPEAKERMODE_PROLOGIC<br>
        ------------------------------------------------------<br>
        This mode is for mono, stereo, 5.1 and 7.1 speaker arrangements, as it is backwards and forwards compatible with stereo, 
        but to get a surround effect a Dolby Prologic or Prologic 2 hardware decoder / amplifier is needed.<br>
        Pan behaviour is the same as SPEAKERMODE_5POINT1.<br>
        <br>
        If this function is called the channel setting in System::setOutputFormat / System_SetOutputFormat is overwritten.<br>
        <br>
        For 3D sounds, panning is determined at runtime by the 3D subsystem based on the speaker mode to determine which speaker the 
        sound should be placed in.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        System::setSpeakerMode
        System::getSpeakerMode
        Channel::setSpeakerLevels
    ]
    */
    public enum SPEAKERMODE
    {
        RAW,           /* There is no specific speakermode.  Sound channels are mapped in order of input to output.  See remarks for more information. */
        MONO,          /* The speakers are monaural. */
        STEREO,        /* The speakers are stereo (default value). */
        _4POINT1,      /* 4.1 speaker setup.  This includes front, center, left, rear and a subwoofer. Also known as a "quad" speaker configuration. */
        _5POINT1,      /* 5.1 speaker setup.  This includes front, center, left, rear left, rear right and a subwoofer. */
        _7POINT1,      /* 7.1 speaker setup.  This includes front, center, left, rear left, rear right, side left, side right and a subwoofer. */
        PROLOGIC,      /* Stereo output, but data is encoded in a way that is picked up by a Prologic/Prologic2 decoder and split into a 5.1 speaker setup. */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These are speaker types defined for use with the Channel::setSpeakerLevels / Channel_SetSpeakerLevels command.

        [REMARKS]
        If you are using SPEAKERMODE_NONE and speaker assignments are meaningless, just cast a raw integer value to this type.<br>
        For example (FMOD_SPEAKER)7 would use the 7th speaker (also the same as SPEAKER_SIDE_RIGHT).  
        Values higher than this can be used if an output system has more than 8 speaker types / output channels.  15 is the current maximum.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        Channel::setSpeakerLevels
        Channel::getSpeakerLevels
    ]
    */
    public enum SPEAKER
    {
        FRONT_LEFT,
        FRONT_RIGHT,
        FRONT_CENTER,
        LOW_FREQUENCY,
        BACK_LEFT,
        BACK_RIGHT,
        SIDE_LEFT,
        SIDE_RIGHT,
        MAX
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These are plugin types defined for use with the System::getNumPlugins / System_GetNumPlugins, 
        System::getPluginInfo / System_GetPluginInfo and System::unloadPlugin / System_UnloadPlugin functions.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        System::getNumPlugins
        System::getPluginInfo
        System::unloadPlugin
    ]
    */
    public enum PLUGINTYPE
    {
        OUTPUT,     /* The plugin type is an output module.  FMOD mixed audio will play through one of these devices */
        CODEC,      /* The plugin type is a file format codec.  FMOD will use these codecs to load file formats for playback. */
        DSP         /* The plugin type is a DSP unit.  FMOD will use these plugins as part of its DSP network to apply effects to output or generate sound in realtime. */
    }


    /*
    [ENUM] 
    [
        [DESCRIPTION]   
        Initialization flags.  Use them with System::init in the flags parameter to change various behaviour.  

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        System::init
    ]
    */
    public enum INITFLAG
    {
        NORMAL                  = 0x00000000,   /* All platforms - Initialize normally */
        NONREALTIME             = 0x00000001,   /* All platforms - Output is driven with System::update. */
        _3D_RIGHTHANDED          = 0x00000002,   /* All platforms - FMOD will treat +X as left, +Y as up and +Z as forwards. */
        DISABLESOFTWARE         = 0x00000004,   /* All platforms - Disable software mixer to save memory.  Anything created with FMOD_SOFTWARE will fail and DSP will not work. */
        DSOUND_DEFERRED         = 0x00000100,   /* Win32 only - for DirectSound output - 3D commands are batched together and executed at System::update. */
        DSOUND_HRTFNONE         = 0x00000200,   /* Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use simple stereo panning/doppler/attenuation when 3D hardware acceleration is not present. */
        DSOUND_HRTFLIGHT        = 0x00000400,   /* Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use a slightly higher quality algorithm when 3D hardware acceleration is not present. */
        DSOUND_HRTFFULL         = 0x00000800,   /* Win32 only - for DirectSound output - FMOD_HARDWARE | FMOD_3D buffers use full quality 3D playback when 3d hardware acceleration is not present. */
        PS2_DISABLECORE0REVERB  = 0x00010000,   /* PS2 only - Disable reverb on CORE 0 to regain SRAM. */
        PS2_DISABLECORE1REVERB  = 0x00020000,   /* PS2 only - Disable reverb on CORE 1 to regain SRAM. */
        XBOX_REMOVEHEADROOM     = 0x00100000,   /* XBox only - By default DirectSound attenuates all sound by 6db to avoid clipping/distortion.  CAUTION.  If you use this flag you are responsible for the final mix to make sure clipping / distortion doesn't happen. */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These definitions describe the type of song being played.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        Sound::getFormat
    ]
    */
    public enum SOUND_TYPE
    {
        UNKNOWN,    /* 3rd party / unknown plugin format. */
        AIFF,       /* AIFF */
        ASF,        /* Microsoft Advanced Systems Format (ie WMA/ASF/WMV) */
        AAC,        /* AAC */
        CDDA,       /* Digital CD audio */
        DLS,        /* Sound font / downloadable sound bank. */
        FLAC,       /* FLAC lossless codec. */
        FSB,        /* FMOD Sample Bank */
        GCADPCM,    /* GameCube ADPCM */
        IT,         /* Impulse Tracker. */
        MIDI,       /* MIDI */
        MOD,        /* Protracker / Fasttracker MOD. */
        MPEG,       /* MP2/MP3 MPEG. */
        OGGVORBIS,  /* Ogg vorbis. */
        S3M,        /* ScreamTracker 3. */
        SF2,        /* Sound font 2 format. */
        RAW,        /* Raw PCM data. */
        USER,       /* User created sound */
        WAV,        /* Microsoft WAV. */
        XM          /* FastTracker 2 XM. */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These definitions describe the native format of the hardware or software buffer that will be used.

        [REMARKS]
        This is the format the native hardware or software buffer will be or is created in.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        System::createSoundEx
        Sound::getFormat
    ]
    */
    public enum SOUND_FORMAT
    {
        NONE,     /* Unitialized / unknown */
        PCM8,     /* 8bit integer PCM data */
        PCM16,    /* 16bit integer PCM data  */
        PCM24,    /* 24bit integer PCM data  */
        PCM32,    /* 32bit integer PCM data  */
        PCMFLOAT, /* 32bit floating point PCM data  */
        GCADPCM,  /* Compressed GameCube DSP data */
        XADPCM,   /* Compressed XBox ADPCM data */
        VAG       /* Compressed PlayStation 2 ADPCM data */
    }


    /*
    [ENUM]
    [  
        [DESCRIPTION]   
        Sound description bitfields, OR them together for loading and describing sounds.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
    ]
    */
    public enum MODE
    {
        DEFAULT             = 0x00000000,  /* default */
        LOOP_OFF            = 0x00000001,  /* For non looping sounds.  Overrides FMOD_LOOP_NORMAL / FMOD_LOOP_BIDI. */
        LOOP_NORMAL         = 0x00000002,  /* For forward looping sounds. */
        LOOP_BIDI           = 0x00000004,  /* For bidirectional looping sounds. (only works on software mixed static sounds). */
        _2D                 = 0x00000008,  /* Ignores any 3d processing. (default). */
        _3D                 = 0x00000010,  /* Makes the sound positionable in 3D.  Overrides FMOD_2D. */
        HARDWARE            = 0x00000020,  /* Attempts to make sounds use hardware acceleration. (default). */
        SOFTWARE            = 0x00000040,  /* Makes sound reside in software.  Overrides FMOD_HARDWARE.  Use this for FFT, DSP, 2D multi speaker support and other software related features. */
        CREATESTREAM        = 0x00000080,  /* Decompress at runtime, streaming from the source provided (standard stream).  Overrides FMOD_CREATESAMPLE. */
        CREATESAMPLE        = 0x00000100,  /* Decompress at loadtime, decompressing or decoding whole file into memory as the target sample format. (standard sample). */
  //    CREATESTATICSTREAM  = 0x00000200,  /* Decompress at runtime, after loading into memory first. - NOT IMPLEMENTED YET */
        OPENUSER            = 0x00000400,  /* Opens a user created static sample or stream. */
        OPENMEMORY          = 0x00000800,  /* "name_or_data" will be interpreted as a pointer to memory instead of filename for creating sounds. */
        OPENRAW             = 0x00001000,  /* Will ignore file format and treat as raw pcm.  User may need to declare if data is FMOD_SIGNED or FMOD_UNSIGNED */
        OPENONLY            = 0x00002000,  /* Just open the file, dont prebuffer or read.  Good for fast opens for info, or when sound::readData is to be used. */
        ACCURATETIME        = 0x00004000,  /* For FMOD_CreateSound - for accurate Sound::getLengthMs/Channel::setTime on VBR MP3, AAC and MOD/S3M/XM/IT/MIDI files.  Scans file first, so takes longer to open. FMOD_OPENONLY does not affect this. */
        MPEGHALFRATE        = 0x00008000,  /* For FMODCE only - Decodes mpeg sounds using a lower quality decode, but faster execution */
        MPEGSEARCH          = 0x00010000,  /* For corrupted / bad MP3 files.  This will search all the way through the file until it hits a valid MPEG header.  Normally only searches for 4k. */
        NONBLOCKING         = 0x00020000,  /* For opening sounds asyncronously, return value from open function must be polled for when it is ready. */
        UNIQUE              = 0x00040000,  /* Unique sound, can only be played one at a time */
        _3D_HEADRELATIVE    = 0x00080000,  /* Make the sound's position, velocity and orientation relative to the listener's position, velocity and orientation */
        _3D_WORLDRELATIVE   = 0x00100000,  /* Make the sound's position, velocity and orientation absolute. */
        CDDA_FORCEASPI      = 0x00200000,  /* For CDDA sounds only - use ASPI instead of NTSCSI to access the specified CD/DVD device. */
        CDDA_JITTERCORRECT  = 0x00400000,  /* For CDDA sounds only - perform jitter correction. Jitter correction helps produce a more accurate CDDA stream at the cost of more CPU time. */
        UNICODE             = 0x00800000,  /* Filename is double-byte unicode. */

        NORMAL              = (HARDWARE | LOOP_OFF | _2D)  /* FMOD_NORMAL is a default sound type.  Loop off, hardware accelerated, and 2D. */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These values describe what state a sound is in after NONBLOCKING has been used to open it.

        [REMARKS]    

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        Sound::getOpenState
        MODE
    ]
    */
    public enum OPENSTATE
    {
        READY = 0,       /* Opened and ready to play */
        LOADING,         /* Initial load in progress */
        LOADINGSUBSOUND, /* Subsound loading in progress */
        ERROR,           /* Failed to open - file not found, out of memory etc.  See return value of Sound::getOpenState for what happened. */
        CONNECTING,      /* Connecting to remote host (internet sounds only) */
        BUFFERING        /* Buffering data */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        These callback types are used with Channel::setCallback.

        [REMARKS]
        Each callback has commanddata parameters passed int unique to the type of callback.<br>
        See reference to FMOD_CHANNEL_CALLBACK to determine what they might mean for each type of callback.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        Channel::setCallback
        FMOD_CHANNEL_CALLBACK
    ]
    */
    public enum CHANNEL_CALLBACKTYPE
    {
        END,                  /* Called when a sound ends. */
        VIRTUALVOICE,         /* Called when a voice is swapped out or swapped in. */

        WAVSYNC,              /* Not implemented */
        MODZXX,               /* Not implemented */
        MODROW,               /* Not implemented */
        MODORDER,             /* Not implemented */
        MODINST,              /* Not implemented */
        MAX
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of windowing methods used in spectrum analysis to reduce leakage / transient signals intefering with the analysis.<br>
        This is a problem with analysis of continuous signals that only have a small portion of the signal sample (the fft window size).<br>
        Windowing the signal with a curve or triangle tapers the sides of the fft window to help alleviate this problem.

        [REMARKS]
        Cyclic signals such as a sine wave that repeat their cycle in a multiple of the window size do not need windowing.<br>
        I.e. If the sine wave repeats every 1024, 512, 256 etc samples and the FMOD fft window is 1024, then the signal would not need windowing.<br>
        Not windowing is the same as FMOD_DSP_FFT_WINDOW_RECT, which is the default.<br>
        If the cycle of the signal (ie the sine wave) is not a multiple of the window size, it will cause frequency abnormalities, so a different windowing method is needed.<br>
        <exclude>
        <br>
        FMOD_DSP_FFT_WINDOW_RECT.<br>
        <img src = "rectangle.gif"></img><br>
        <br>
        FMOD_DSP_FFT_WINDOW_TRIANGLE.<br>
        <img src = "triangle.gif"></img><br>
        <br>
        FMOD_DSP_FFT_WINDOW_HAMMING.<br>
        <img src = "hamming.gif"></img><br>
        <br>
        FMOD_DSP_FFT_WINDOW_HANNING.<br>
        <img src = "hanning.gif"></img><br>
        <br>
        FMOD_DSP_FFT_WINDOW_BLACKMAN.<br>
        <img src = "blackman.gif"></img><br>
        <br>
        FMOD_DSP_FFT_WINDOW_BLACKMANHARRIS.<br>
        <img src = "blackmanharris.gif"></img>
        </exclude>

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        System::getSpectrum
        Channel::getSpectrum
    ]
    */
    public enum DSP_FFT_WINDOW
    {
        RECT,           /* w[n] = 1.0                                                                                            */
        TRIANGLE,       /* w[n] = TRI(2n/N)                                                                                      */
        HAMMING,        /* w[n] = 0.54 - (0.46 * COS(n/N) )                                                                      */
        HANNING,        /* w[n] = 0.5 *  (1.0  - COS(n/N) )                                                                      */
        BLACKMAN,       /* w[n] = 0.42 - (0.5  * COS(n/N) ) + (0.08 * COS(2.0 * n/N) )                                           */
        BLACKMANHARRIS, /* w[n] = 0.35875 - (0.48829 * COS(1.0 * n/N)) + (0.14128 * COS(2.0 * n/N)) - (0.01168 * COS(3.0 * n/N)) */

        MAX
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of tag types that could be stored within a sound.  These include id3 tags, metadata from netstreams and vorbis/asf data.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        Sound::getTag
    ]
    */
    public enum TAGTYPE
    {
        UNKNOWN = 0,
        ID3V1,
        ID3V2,
        VORBISCOMMENT,
        SHOUTCAST,
        ICECAST,
        ASF,
        MIDI,
        FMOD,
        USER
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of data types that can be returned by Sound::getTag

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        Sound::getTag
    ]
    */
    public enum TAGDATATYPE
    {
        BINARY = 0,
        INT,
        FLOAT,
        STRING,
        STRING_UTF16,
        STRING_UTF16BE,
        STRING_UTF8,
        CDTOC
    }


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]   
        Structure describing a piece of tag data.

        [REMARKS]
        Members marked with [in] mean the user sets the value before passing it to the function.<br>
        Members marked with [out] mean FMOD sets the value to be used after the function exits.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        Sound::getTag
        TAGTYPE
        TAGDATATYPE
    ]
    */
    public struct TAG
    {
        public TAGTYPE           type;         /* [out] The type of this tag. */
        public TAGDATATYPE       datatype;     /* [out] The type of data that this tag contains */
        public string            name;         /* [out] The name of this tag i.e. "TITLE", "ARTIST" etc. */
        public IntPtr            data;         /* [out] Pointer to the tag data - its format is determined by the datatype member */
        public uint              datalen;      /* [out] Length of the data contained in this tag */
        public bool              updated;      /* [out] True if this tag has been updated since last being accessed with Sound::getTag */
    }


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]   
        Structure describing a CD/DVD table of contents

        [REMARKS]
        Members marked with [in] mean the user sets the value before passing it to the function.<br>
        Members marked with [out] mean FMOD sets the value to be used after the function exits.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        Sound::getTag
    ]
    */
    public struct CDTOC
    {
        public int numtracks;                  /* [out] The number of tracks on the CD */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=100)]
        public int[] min;                   /* [out] The start offset of each track in minutes */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=100)]
        public int[] sec;                   /* [out] The start offset of each track in seconds */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=100)]
        public int[] frame;                 /* [out] The start offset of each track in frames */
    }


    /*
    [ENUM]
    [
        [DESCRIPTION]   
        List of time types that can be returned by Sound::getLength and used with Channel::setPosition or Channel::getPosition.

        [REMARKS]

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]      
        Sound::getLength
        Channel::setPosition
        Channel::getPosition
    ]
    */
    public enum TIMEUNIT
    {
        MS        = 0x00000001,  /* Milliseconds. */
        PCM       = 0x00000002,  /* PCM Samples, related to milliseconds * samplerate / 1000. */
        PCMBYTES  = 0x00000004,  /* Bytes, related to PCM samples * channels * datawidth (ie 16bit = 2 bytes). */
        RAWBYTES  = 0x00000008,  /* Raw file bytes of (compressed) sound data (does not include headers).  Only used by Sound::getLength and Channel::getPosition. */
        ORDER     = 0x00000100,  /* MOD/S3M/XM/IT.  Order in a sequenced module format.  Use Sound::getFormat to determine the format. */
        ROW       = 0x00000200,  /* MOD/S3M/XM/IT.  Current row in a sequenced module format.  Sound::getLength will return the number if rows in the currently playing or seeked to pattern. */
    }

    public delegate RESULT SOUND_NONBLOCKCALLBACK (IntPtr soundraw, RESULT result);
    public delegate RESULT SOUND_PCMREADCALLBACK  (IntPtr soundraw, IntPtr data, uint datalen);
    public delegate RESULT SOUND_PCMSETPOSCALLBACK(IntPtr soundraw, int subsound, uint position, TIMEUNIT postype);


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Use this structure with System::createSound / System_CreateSound when more control is needed over loading.<br>
        The possible reasons to use this with System::createSound / System_CreateSound are:<br>
        <li>Loading a file from memory.
        <li>Loading a file from within another larger (possibly wad/pak) file, by giving the loader an offset and length.
        <li>To create a user created / non file based sound.
        <li>To specify a starting subsound to seek to within a multi-sample sounds (ie FSB/DLS/SF2) when created as a stream.
        <li>To specify which subsounds to load for multi-sample sounds (ie FSB/DLS/SF2) so that memory is saved and only a subset is actually loaded/read from disk.
        <li>To specify 'piggyback' read and seek callbacks for capture of sound data as fmod reads and decodes it.  Useful for ripping decoded PCM data from sounds as they are loaded / played.
        <li>To specify a midi DLS/SF2 sampleset file to load when opening a midi file.

        [REMARKS]
        This structure is optional!  Specify 0 or NULL in System::createSound / System_CreateSound if you don't need it!<br>
        <br>
        Members marked with [in] mean the user sets the value before passing it to the function.<br>
        Members marked with [out] mean FMOD sets the value to be used after the function exits.<br>

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        System::createSound
    ]
    */
    public struct CREATESOUNDEXINFO
    {
        public int                         cbsize;                 /* [in] Size of this structure.  This is used so the structure can be expanded in the future and still work on older versions of FMOD Ex. */
        public int                         length;                 /* [in] Optional. Specify 0 to ignore. Size in bytes of file to load, or sound to create (in this case only if FMOD_OPENUSER is used).  Required if loading from memory.  If 0 is specified, then it will use the size of the file (unless loading from memory then an error will be returned). */
        public uint                        fileoffset;             /* [in] Optional. Specify 0 to ignore. Offset from start of the file to start loading from.  This is useful for loading files from inside big data files. */
        public int                         numchannels;            /* [in] Optional. Specify 0 to ignore. Number of channels in a sound specified only if OPENUSER is used. */
        public int                         defaultfrequency;       /* [in] Optional. Specify 0 to ignore. Default frequency of sound in a sound specified only if OPENUSER is used.  Other formats use the frequency determined by the file format. */
        public SOUND_FORMAT                format;                 /* [in] Optional. Specify 0 or SOUND_FORMAT_NONE to ignore. Format of the sound specified only if OPENUSER is used.  Other formats use the format determined by the file format.   */
        public int                         initialsubsound;        /* [in] Optional. Specify 0 to ignore. In a multi-sample file format such as .FSB/.DLS/.SF2, specify the initial subsound to seek to, only if CREATESTREAM is used. */
        public int                         numsubsounds;           /* [in] Optional. Specify 0 to ignore or have no subsounds.  In a user created multi-sample sound, specify the number of subsounds within the sound that are accessable with Sound::getSubSound / SoundGetSubSound. */
        public IntPtr                      inclusionlist;          /* [in] Optional. Specify 0 or NULL to ignore.  In a multi-sample format such as .FSB/.DLS/.SF2 it may be desirable to specify only a subset of sounds to be loaded out of the whole file.  This is an array of subsound indicies to load into memory when created. */
        public int                         inclusionlistnum;       /* [in] Optional. Specify 0 to ignore. This is the number of integers contained within the */
        public SOUND_PCMREADCALLBACK       pcmreadcallback;        /* [in] Optional. Specify 0 to ignore. Callback to 'piggyback' on FMOD's read functions and accept or even write PCM data while FMOD is opening the sound.  Used for user sounds created with OPENUSER or for capturing decoded data as FMOD reads it. */
        public SOUND_PCMSETPOSCALLBACK     pcmsetposcallback;      /* [in] Optional. Specify 0 to ignore. Callback for when the user calls a seeking function such as Channel::setPosition within a multi-sample sound, and for when it is opened.*/
        public SOUND_NONBLOCKCALLBACK      nonblockcallback;       /* [in] Optional. Specify 0 to ignore. Callback for successful completion, or error while loading a sound that used the FMOD_NONBLOCKING flag.*/
        public string                      dlsname;                /* [in] Optional. Specify 0 to ignore. Filename for a MIDI dls sample set, when loading a midi file.   If not specified on windows it will attempt to open /windows/system32/drivers/gm.dls, otherwise the midi will fail to open.  */
        public string                      encryptionkey;          /* [in] Optional. Specify 0 to ignore. Key for encrypted FSB file.  Without this key an encrypted FSB file will not load. */
        public int                         maxpolyphony;           /* [in] Optional. Specify 0 to ingore. For sequenced formats with dynamic channel allocation such as .MID and .IT, this specifies the maximum voice count allowed while playing.  .IT defaults to 64.  .MID defaults to 32. */
        public IntPtr                      userdata;               /* [in] Optional. Specify 0 to ignore. This is user data to be attached to the sound during creation.  Access via Sound::getUserData. */

    }


    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Structure defining a reverb environment.<br>
        <br>
        For more indepth descriptions of the reverb properties under win32, please see the EAX2 and EAX3
        documentation at http://developer.creative.com/ under the 'downloads' section.<br>
        If they do not have the EAX3 documentation, then most information can be attained from
        the EAX2 documentation, as EAX3 only adds some more parameters and functionality on top of 
        EAX2.

        [REMARKS]
        Note the default reverb properties are the same as the FMOD_PRESET_GENERIC preset.<br>
        Note that integer values that typically range from -10,000 to 1000 are represented in 
        decibels, and are of a logarithmic scale, not linear, wheras FMOD_FLOAT values are always linear.<br>
        PORTABILITY: Each member has the platform it supports in braces ie (win32/xbox).  
        Some reverb parameters are only supported in win32 and some only on xbox. If all parameters are set then
        the reverb should product a similar effect on either platform.<br>
        Win32/Win64 - This is only supported with FMOD_OUTPUT_DSOUND and EAX compatible sound cards. <br>
        Macintosh - This is only supported with FMOD_OUTPUT_OPENAL and EAX compatible sound cards. <br>
        Linux - This is only supported with FMOD_OUTPUT_OPENAL and EAX compatible sound cards. <br>
        XBox - Only a subset of parameters are supported. <br> 
        PlayStation 2 - Only the Environment and Flags paramenters are supported. <br>
        <br>
        The numerical values listed below are the maximum, minimum and default values for each variable respectively.<br>
        <br>
        Members marked with [in] mean the user sets the value before passing it to the function.<br>
        Members marked with [out] mean FMOD sets the value to be used after the function exits.<br>

        [PLATFORMS]
        Win32, Win64, Macintosh, XBox, PlayStation 2

        [SEE_ALSO]
        System::setReverbProperties
        System::getReverbProperties
        REVERB_PRESETS
        REVERB_FLAGS
    ]
    */
    public class REVERB_PROPERTIES
    {                                   /*          MIN     MAX    DEFAULT   DESCRIPTION */
        public int   Instance;          /* [in]     0     , 2     , 0      , EAX4 only. Environment Instance. 3 seperate reverbs simultaneously are possible. This specifies which one to set. (win32 only) */
        public uint  Environment;       /* [in/out] 0     , 25    , 0      , sets all listener properties (win32/ps2) */
        public float EnvSize;           /* [in/out] 1.0   , 100.0 , 7.5    , environment size in meters (win32 only) */
        public float EnvDiffusion;      /* [in/out] 0.0   , 1.0   , 1.0    , environment diffusion (win32/xbox) */
        public int   Room;              /* [in/out] -10000, 0     , -1000  , room effect level (at mid frequencies) (win32/xbox) */
        public int   RoomHF;            /* [in/out] -10000, 0     , -100   , relative room effect level at high frequencies (win32/xbox) */
        public int   RoomLF;            /* [in/out] -10000, 0     , 0      , relative room effect level at low frequencies (win32 only) */
        public float DecayTime;         /* [in/out] 0.1   , 20.0  , 1.49   , reverberation decay time at mid frequencies (win32/xbox) */
        public float DecayHFRatio;      /* [in/out] 0.1   , 2.0   , 0.83   , high-frequency to mid-frequency decay time ratio (win32/xbox) */
        public float DecayLFRatio;      /* [in/out] 0.1   , 2.0   , 1.0    , low-frequency to mid-frequency decay time ratio (win32 only) */
        public int   Reflections;       /* [in/out] -10000, 1000  , -2602  , early reflections level relative to room effect (win32/xbox) */
        public float ReflectionsDelay;  /* [in/out] 0.0   , 0.3   , 0.007  , initial reflection delay time (win32/xbox) */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=3)]
        public float[] ReflectionsPan;  /* [in/out]       ,       , [0,0,0], early reflections panning vector (win32 only) */
        public int   Reverb;            /* [in/out] -10000, 2000  , 200    , late reverberation level relative to room effect (win32/xbox) */
        public float ReverbDelay;       /* [in/out] 0.0   , 0.1   , 0.011  , late reverberation delay time relative to initial reflection (win32/xbox) */
        [MarshalAs(UnmanagedType.ByValArray,SizeConst=3)]
        public float[] ReverbPan;       /* [in/out]       ,       , [0,0,0], late reverberation panning vector (win32 only) */
        public float EchoTime;          /* [in/out] .075  , 0.25  , 0.25   , echo time (win32 only) */
        public float EchoDepth;         /* [in/out] 0.0   , 1.0   , 0.0    , echo depth (win32 only) */
        public float ModulationTime;    /* [in/out] 0.04  , 4.0   , 0.25   , modulation time (win32 only) */
        public float ModulationDepth;   /* [in/out] 0.0   , 1.0   , 0.0    , modulation depth (win32 only) */
        public float AirAbsorptionHF;   /* [in/out] -100  , 0.0   , -5.0   , change in level per meter at high frequencies (win32 only) */
        public float HFReference;       /* [in/out] 1000.0, 20000 , 5000.0 , reference high frequency (hz) (win32/xbox) */
        public float LFReference;       /* [in/out] 20.0  , 1000.0, 250.0  , reference low frequency (hz) (win32 only) */
        public float RoomRolloffFactor; /* [in/out] 0.0   , 10.0  , 0.0    , like 3D_Listener_SetRolloffFactor but for room effect (win32/xbox) */
        public float Diffusion;         /* [in/out] 0.0   , 100.0 , 100.0  , Value that controls the echo density in the late reverberation decay. (xbox only) */
        public float Density;           /* [in/out] 0.0   , 100.0 , 100.0  , Value that controls the modal density in the late reverberation decay (xbox only) */
        public uint  Flags;             /* [in/out] REVERB_FLAGS - modifies the behavior of above properties (win32/ps2) */

        #region wrapperinternal
        public REVERB_PROPERTIES(int instance, uint environment, float envSize, float envDiffusion, int room, int roomHF, int roomLF,
                          float decayTime, float decayHFRatio, float decayLFRatio, int reflections, float reflectionsDelay,
                          float reflectionsPanx, float reflectionsPany, float reflectionsPanz, int reverb, float reverbDelay,
                          float reverbPanx, float reverbPany, float reverbPanz, float echoTime, float echoDepth, float modulationTime,
                          float modulationDepth, float airAbsorptionHF, float hfReference, float lfReference, float roomRolloffFactor,
                          float diffusion, float density, uint flags)
        {
            Instance            = instance;
            Environment         = environment;
            EnvSize             = envSize;
            EnvDiffusion        = envDiffusion;
            Room                = room;
            RoomHF              = roomHF;
            RoomLF              = roomLF;
            DecayTime           = decayTime;
            DecayHFRatio        = decayHFRatio;
            DecayLFRatio        = decayLFRatio;
            Reflections         = reflections;
            ReflectionsDelay    = reflectionsDelay;
            ReflectionsPan[0]   = reflectionsPanx;
            ReflectionsPan[1]   = reflectionsPany;
            ReflectionsPan[2]   = reflectionsPanz;
            Reverb              = reverb;
            ReverbDelay          = reverbDelay;
            ReverbPan[0]        = reverbPanx;
            ReverbPan[1]        = reverbPany;
            ReverbPan[2]        = reverbPanz;
            EchoTime            = echoTime;
            EchoDepth           = echoDepth;
            ModulationTime      = modulationTime;
            ModulationDepth     = modulationDepth;
            AirAbsorptionHF     = airAbsorptionHF;
            HFReference         = hfReference;
            LFReference         = lfReference;
            RoomRolloffFactor   = roomRolloffFactor;
            Diffusion           = diffusion;
            Density             = density;
            Flags               = flags;
        }
        #endregion
    }


    /*
    [DEFINE] 
    [
        [NAME] 
        REVERB_FLAGS

        [DESCRIPTION]
        Values for the Flags member of the REVERB_PROPERTIES structure.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        REVERB_PROPERTIES
    ]
    */
    public class REVERB_FLAGS
    {
        public const uint DECAYTIMESCALE        = 0x00000001;   /* 'EnvSize' affects reverberation decay time */
        public const uint REFLECTIONSSCALE      = 0x00000002;   /* 'EnvSize' affects reflection level */
        public const uint REFLECTIONSDELAYSCALE = 0x00000004;   /* 'EnvSize' affects initial reflection delay time */
        public const uint REVERBSCALE           = 0x00000008;   /* 'EnvSize' affects reflections level */
        public const uint REVERBDELAYSCALE      = 0x00000010;   /* 'EnvSize' affects late reverberation delay time */
        public const uint DECAYHFLIMIT          = 0x00000020;   /* AirAbsorptionHF affects DecayHFRatio */
        public const uint ECHOTIMESCALE         = 0x00000040;   /* 'EnvSize' affects echo time */
        public const uint MODULATIONTIMESCALE   = 0x00000080;   /* 'EnvSize' affects modulation time */
        public const uint DEFAULT               = (DECAYTIMESCALE | 
            REFLECTIONSSCALE | 
            REFLECTIONSDELAYSCALE | 
            REVERBSCALE | 
            REVERBDELAYSCALE | 
            DECAYHFLIMIT);
    }


    /*
    [DEFINE] 
    [
    [NAME] 
    FMOD_REVERB_PRESETS

    [DESCRIPTION]   
    A set of predefined environment PARAMETERS, created by Creative Labs
    These are used to initialize an FMOD_REVERB_PROPERTIES structure statically.
    ie 
    FMOD_REVERB_PROPERTIES prop = FMOD_PRESET_GENERIC;

    [PLATFORMS]
    Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

    [SEE_ALSO]
    System::setReverbProperties
    ]
    */
    class PRESET
    {
        /*                                                                           Instance  Env   Size    Diffus  Room   RoomHF  RmLF DecTm   DecHF  DecLF   Refl  RefDel  RefPan           Revb  RevDel  ReverbPan       EchoTm  EchDp  ModTm  ModDp  AirAbs  HFRef    LFRef  RRlOff Diffus  Densty  FLAGS */
        public REVERB_PROPERTIES OFF()                 { return new REVERB_PROPERTIES(0,       0,    7.5f,   1.00f, -10000, -10000, 0,   1.00f,  1.00f, 1.0f,  -2602, 0.007f, 0.0f,0.0f,0.0f,   200, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,   0.0f,   0.0f, 0x3f );}
        public REVERB_PROPERTIES GENERIC()             { return new REVERB_PROPERTIES(0,       0,    7.5f,   1.00f, -1000,  -100,   0,   1.49f,  0.83f, 1.0f,  -2602, 0.007f, 0.0f,0.0f,0.0f,   200, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES PADDEDCELL()          { return new REVERB_PROPERTIES(0,       1,    1.4f,   1.00f, -1000,  -6000,  0,   0.17f,  0.10f, 1.0f,  -1204, 0.001f, 0.0f,0.0f,0.0f,   207, 0.002f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES ROOM()                { return new REVERB_PROPERTIES(0,       2,    1.9f,   1.00f, -1000,  -454,   0,   0.40f,  0.83f, 1.0f,  -1646, 0.002f, 0.0f,0.0f,0.0f,    53, 0.003f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES BATHROOM()            { return new REVERB_PROPERTIES(0,       3,    1.4f,   1.00f, -1000,  -1200,  0,   1.49f,  0.54f, 1.0f,   -370, 0.007f, 0.0f,0.0f,0.0f,  1030, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f,  60.0f, 0x3f );}
        public REVERB_PROPERTIES LIVINGROOM()          { return new REVERB_PROPERTIES(0,       4,    2.5f,   1.00f, -1000,  -6000,  0,   0.50f,  0.10f, 1.0f,  -1376, 0.003f, 0.0f,0.0f,0.0f, -1104, 0.004f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES STONEROOM()           { return new REVERB_PROPERTIES(0,       5,    11.6f,  1.00f, -1000,  -300,   0,   2.31f,  0.64f, 1.0f,   -711, 0.012f, 0.0f,0.0f,0.0f,    83, 0.017f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES AUDITORIUM()          { return new REVERB_PROPERTIES(0,       6,    21.6f,  1.00f, -1000,  -476,   0,   4.32f,  0.59f, 1.0f,   -789, 0.020f, 0.0f,0.0f,0.0f,  -289, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES CONCERTHALL()         { return new REVERB_PROPERTIES(0,       7,    19.6f,  1.00f, -1000,  -500,   0,   3.92f,  0.70f, 1.0f,  -1230, 0.020f, 0.0f,0.0f,0.0f,    -2, 0.029f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES CAVE()                { return new REVERB_PROPERTIES(0,       8,    14.6f,  1.00f, -1000,  0,      0,   2.91f,  1.30f, 1.0f,   -602, 0.015f, 0.0f,0.0f,0.0f,  -302, 0.022f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES ARENA()               { return new REVERB_PROPERTIES(0,       9,    36.2f,  1.00f, -1000,  -698,   0,   7.24f,  0.33f, 1.0f,  -1166, 0.020f, 0.0f,0.0f,0.0f,    16, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES HANGAR()              { return new REVERB_PROPERTIES(0,       10,   50.3f,  1.00f, -1000,  -1000,  0,   10.05f, 0.23f, 1.0f,   -602, 0.020f, 0.0f,0.0f,0.0f,   198, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES CARPETTEDHALLWAY()    { return new REVERB_PROPERTIES(0,       11,   1.9f,   1.00f, -1000,  -4000,  0,   0.30f,  0.10f, 1.0f,  -1831, 0.002f, 0.0f,0.0f,0.0f, -1630, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES HALLWAY()             { return new REVERB_PROPERTIES(0,       12,   1.8f,   1.00f, -1000,  -300,   0,   1.49f,  0.59f, 1.0f,  -1219, 0.007f, 0.0f,0.0f,0.0f,   441, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES STONECORRIDOR()       { return new REVERB_PROPERTIES(0,       13,   13.5f,  1.00f, -1000,  -237,   0,   2.70f,  0.79f, 1.0f,  -1214, 0.013f, 0.0f,0.0f,0.0f,   395, 0.020f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES ALLEY()               { return new REVERB_PROPERTIES(0,       14,   7.5f,   0.30f, -1000,  -270,   0,   1.49f,  0.86f, 1.0f,  -1204, 0.007f, 0.0f,0.0f,0.0f,    -4, 0.011f, 0.0f,0.0f,0.0f, 0.125f, 0.95f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES FOREST()              { return new REVERB_PROPERTIES(0,       15,   38.0f,  0.30f, -1000,  -3300,  0,   1.49f,  0.54f, 1.0f,  -2560, 0.162f, 0.0f,0.0f,0.0f,  -229, 0.088f, 0.0f,0.0f,0.0f, 0.125f, 1.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  79.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES CITY()                { return new REVERB_PROPERTIES(0,       16,   7.5f,   0.50f, -1000,  -800,   0,   1.49f,  0.67f, 1.0f,  -2273, 0.007f, 0.0f,0.0f,0.0f, -1691, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  50.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES MOUNTAINS()           { return new REVERB_PROPERTIES(0,       17,   100.0f, 0.27f, -1000,  -2500,  0,   1.49f,  0.21f, 1.0f,  -2780, 0.300f, 0.0f,0.0f,0.0f, -1434, 0.100f, 0.0f,0.0f,0.0f, 0.250f, 1.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  27.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES QUARRY()              { return new REVERB_PROPERTIES(0,       18,   17.5f,  1.00f, -1000,  -1000,  0,   1.49f,  0.83f, 1.0f, -10000, 0.061f, 0.0f,0.0f,0.0f,   500, 0.025f, 0.0f,0.0f,0.0f, 0.125f, 0.70f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES PLAIN()               { return new REVERB_PROPERTIES(0,       19,   42.5f,  0.21f, -1000,  -2000,  0,   1.49f,  0.50f, 1.0f,  -2466, 0.179f, 0.0f,0.0f,0.0f, -1926, 0.100f, 0.0f,0.0f,0.0f, 0.250f, 1.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  21.0f, 100.0f, 0x3f );}
        public REVERB_PROPERTIES PARKINGLOT()          { return new REVERB_PROPERTIES(0,       20,   8.3f,   1.00f, -1000,  0,      0,   1.65f,  1.50f, 1.0f,  -1363, 0.008f, 0.0f,0.0f,0.0f, -1153, 0.012f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES SEWERPIPE()           { return new REVERB_PROPERTIES(0,       21,   1.7f,   0.80f, -1000,  -1000,  0,   2.81f,  0.14f, 1.0f,    429, 0.014f, 0.0f,0.0f,0.0f,  1023, 0.021f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 0.000f, -5.0f, 5000.0f, 250.0f, 0.0f,  80.0f,  60.0f, 0x3f );}
        public REVERB_PROPERTIES UNDERWATER()          { return new REVERB_PROPERTIES(0,       22,   1.8f,   1.00f, -1000,  -4000,  0,   1.49f,  0.10f, 1.0f,   -449, 0.007f, 0.0f,0.0f,0.0f,  1700, 0.011f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 1.18f, 0.348f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x3f );}

        /* Non I3DL2 presets */

        public REVERB_PROPERTIES DRUGGED()             { return new REVERB_PROPERTIES(0,       23,   1.9f,   0.50f, -1000,  0,      0,   8.39f,  1.39f, 1.0f,  -115,  0.002f, 0.0f,0.0f,0.0f,   985, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 0.25f, 1.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES DIZZY()               { return new REVERB_PROPERTIES(0,       24,   1.8f,   0.60f, -1000,  -400,   0,   17.23f, 0.56f, 1.0f,  -1713, 0.020f, 0.0f,0.0f,0.0f,  -613, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 1.00f, 0.81f, 0.310f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}
        public REVERB_PROPERTIES PSYCHOTIC()           { return new REVERB_PROPERTIES(0,       25,   1.0f,   0.50f, -1000,  -151,   0,   7.56f,  0.91f, 1.0f,  -626,  0.020f, 0.0f,0.0f,0.0f,   774, 0.030f, 0.0f,0.0f,0.0f, 0.250f, 0.00f, 4.00f, 1.000f, -5.0f, 5000.0f, 250.0f, 0.0f, 100.0f, 100.0f, 0x1f );}

        /* PlayStation 2 Only presets */

        public REVERB_PROPERTIES PS2_ROOM()            { return new REVERB_PROPERTIES(0,       1,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_STUDIO_A()        { return new REVERB_PROPERTIES(0,       2,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_STUDIO_B()        { return new REVERB_PROPERTIES(0,       3,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_STUDIO_C()        { return new REVERB_PROPERTIES(0,       4,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_HALL()            { return new REVERB_PROPERTIES(0,       5,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_SPACE()           { return new REVERB_PROPERTIES(0,       6,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_ECHO()            { return new REVERB_PROPERTIES(0,       7,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_DELAY()           { return new REVERB_PROPERTIES(0,       8,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
        public REVERB_PROPERTIES PS2_PIPE()            { return new REVERB_PROPERTIES(0,       9,    0,	    0,         0,  0,      0,   0.0f,   0.0f,  0.0f,     0,  0.000f,  0.0f,0.0f,0.0f ,     0, 0.000f,  0.0f,0.0f,0.0f , 0.000f, 0.00f, 0.00f, 0.000f,  0.0f, 0000.0f,   0.0f, 0.0f,   0.0f,   0.0f, 0x31f );}
    }

    /*
    [STRUCTURE] 
    [
        [DESCRIPTION]
        Structure defining the properties for a reverb source, related to a FMOD channel.

        For more indepth descriptions of the reverb properties under win32, please see the EAX3
        documentation at http://developer.creative.com/ under the 'downloads' section.
        If they do not have the EAX3 documentation, then most information can be attained from
        the EAX2 documentation, as EAX3 only adds some more parameters and functionality on top of 
        EAX2.

        Note the default reverb properties are the same as the PRESET_GENERIC preset.
        Note that integer values that typically range from -10,000 to 1000 are represented in 
        decibels, and are of a logarithmic scale, not linear, wheras FLOAT values are typically linear.
        PORTABILITY: Each member has the platform it supports in braces ie (win32/xbox).  
        Some reverb parameters are only supported in win32 and some only on xbox. If all parameters are set then
        the reverb should product a similar effect on either platform.
        Linux and FMODCE do not support the reverb api.

        The numerical values listed below are the maximum, minimum and default values for each variable respectively.

        [REMARKS]
        For EAX4 support with multiple reverb environments, set FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT0,
        FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT1 or/and FMOD_REVERB_CHANNELFLAGS_ENVIRONMENT2 in the flags member 
        of FMOD_REVERB_CHANNELPROPERTIES to specify which environment instance(s) to target. <br>
        Only up to 2 environments to target can be specified at once. Specifying three will result in an error.
        If the sound card does not support EAX4, the environment flag is ignored.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        Channel::setReverbProperties
        Channel::getReverbProperties
        REVERB_CHANNELFLAGS
    ]
    */
    public struct REVERB_CHANNELPROPERTIES  
    {                                      /*          MIN     MAX    DEFAULT  DESCRIPTION */
        public int   Direct;               /* [in/out] -10000, 1000,  0,       direct path level (at low and mid frequencies) (win32/xbox) */
        public int   DirectHF;             /* [in/out] -10000, 0,     0,       relative direct path level at high frequencies (win32/xbox) */
        public int   Room;                 /* [in/out] -10000, 1000,  0,       room effect level (at low and mid frequencies) (win32/xbox) */
        public int   RoomHF;               /* [in/out] -10000, 0,     0,       relative room effect level at high frequencies (win32/xbox) */
        public int   Obstruction;          /* [in/out] -10000, 0,     0,       main obstruction control (attenuation at high frequencies)  (win32/xbox) */
        public float ObstructionLFRatio;   /* [in/out] 0.0,    1.0,   0.0,     obstruction low-frequency level re. main control (win32/xbox) */
        public int   Occlusion;            /* [in/out] -10000, 0,     0,       main occlusion control (attenuation at high frequencies) (win32/xbox) */
        public float OcclusionLFRatio;     /* [in/out] 0.0,    1.0,   0.25,    occlusion low-frequency level re. main control (win32/xbox) */
        public float OcclusionRoomRatio;   /* [in/out] 0.0,    10.0,  1.5,     relative occlusion control for room effect (win32) */
        public float OcclusionDirectRatio; /* [in/out] 0.0,    10.0,  1.0,     relative occlusion control for direct path (win32) */
        public int   Exclusion;            /* [in/out] -10000, 0,     0,       main exlusion control (attenuation at high frequencies) (win32) */
        public float ExclusionLFRatio;     /* [in/out] 0.0,    1.0,   1.0,     exclusion low-frequency level re. main control (win32) */
        public int   OutsideVolumeHF;      /* [in/out] -10000, 0,     0,       outside sound cone level at high frequencies (win32) */
        public float DopplerFactor;        /* [in/out] 0.0,    10.0,  0.0,     like DS3D flDopplerFactor but per source (win32) */
        public float RolloffFactor;        /* [in/out] 0.0,    10.0,  0.0,     like DS3D flRolloffFactor but per source (win32) */
        public float RoomRolloffFactor;    /* [in/out] 0.0,    10.0,  0.0,     like DS3D flRolloffFactor but for room effect (win32/xbox) */
        public float AirAbsorptionFactor;  /* [in/out] 0.0,    10.0,  1.0,     multiplies AirAbsorptionHF member of REVERB_PROPERTIES (win32) */
        public uint  Flags;                /* [in/out] REVERB_CHANNELFLAGS - modifies the behavior of properties (win32) */
    }


    /*
    [DEFINE] 
    [
        [NAME] 
        REVERB_CHANNELFLAGS

        [DESCRIPTION]
        Values for the Flags member of the REVERB_CHANNELPROPERTIES structure.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        REVERB_CHANNELPROPERTIES
    ]
    */
    public class REVERB_CHANNELFLAGS
    {
        public const uint DIRECTHFAUTO = 0x00000001; /* Automatic setting of 'Direct'  due to distance from listener */
        public const uint ROOMAUTO     = 0x00000002; /* Automatic setting of 'Room'  due to distance from listener */
        public const uint ROOMHFAUTO   = 0x00000004; /* Automatic setting of 'RoomHF' due to distance from listener */
        public const uint ENVIRONMENT0 = 0x00000008; /* EAX4 only. Specify channel to target reverb instance 0. */
        public const uint ENVIRONMENT1 = 0x00000010; /* EAX4 only. Specify channel to target reverb instance 1. */
        public const uint ENVIRONMENT2 = 0x00000020; /* EAX4 only. Specify channel to target reverb instance 2. */
        public const uint DEFAULT      = (DIRECTHFAUTO | ROOMAUTO | ROOMHFAUTO | ENVIRONMENT0);
    }


    /*
    [DEFINE] 
    [
        [NAME] 
        FMOD_MISC_VALUES

        [DESCRIPTION]
        Miscellaneous values for FMOD functions.

        [PLATFORMS]
        Win32, Win64, Linux, Macintosh, XBox, PlayStation 2, GameCube

        [SEE_ALSO]
        System::playSound
        System::playDSP
        System::getChannel
    ]
    */
    public class CHANNELINDEX
    {
        public const int FREE   = -1;     /* For a channel index, FMOD chooses a free voice using the priority system. */
        public const int REUSE  = -2;     /* For a channel index, re-use the channel handle that was passed in. */
        public const int ALL    = -3;     /* For a channel index, this flag will affect ALL channels available!  */
    }


    /* 
        FMOD Callbacks
    */
    public delegate RESULT CHANNEL_CALLBACK      (ref Channel channel, CHANNEL_CALLBACKTYPE type, int command, uint commanddata1, uint commanddata2);

    public delegate RESULT FILE_OPENCALLBACK     (string name, int unicode, ref uint filesize, ref IntPtr handle, ref IntPtr userdata);
    public delegate RESULT FILE_CLOSECALLBACK    (IntPtr handle, IntPtr userdata);
    public delegate RESULT FILE_READCALLBACK     (IntPtr handle, IntPtr buffer, uint sizebytes, ref uint bytesread, IntPtr userdata);
    public delegate RESULT FILE_SEEKCALLBACK     (IntPtr handle, int pos, int type, IntPtr userdata);


    /*
        FMOD System factory functions.  Use this to create an FMOD System Instance.  below you will see System_Init/Close to get started.
    */
    public class Factory
    {        
        public static RESULT System_Create(ref System system)
        {
            RESULT result      = RESULT.OK;
            IntPtr      systemraw   = new IntPtr();
            System      systemnew   = null;

            result = FMOD_System_Create(ref systemraw);
            if (result != RESULT.OK)
            {
                return result;
            }

            systemnew = new System();
            systemnew.setRaw(systemraw);
            system = systemnew;

            return result;
        }


        #region importfunctions
  
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Create                      (ref IntPtr system);

        #endregion
    }


    /*
        'System' API
    */
    public class System
    {
        public RESULT release                ()
        {
            return FMOD_System_Release(systemraw);
        }


        // Pre-init functions.
        public RESULT setOutput              (OUTPUTTYPE output)
        {
            return FMOD_System_SetOutput(systemraw, output);
        }
        public RESULT getOutput              (ref OUTPUTTYPE output)
        {
            return FMOD_System_GetOutput(systemraw, ref output);
        }
        public RESULT setOutputPlugin        (int index)
        {
            return FMOD_System_SetOutputPlugin(systemraw, index);
        }
        public RESULT getOutputPlugin        (ref int index)
        {
            return FMOD_System_GetOutputPlugin(systemraw, ref index);
        }
        public RESULT setOutputFormat        (int samplerate, SOUND_FORMAT format, int numchannels, IntPtr extradata)
        {
            return FMOD_System_SetOutputFormat(systemraw, samplerate, format, numchannels, extradata);
        }
        public RESULT getOutputFormat        (ref int samplerate, ref SOUND_FORMAT format, ref int numchannels, ref int bits, ref IntPtr extradata)
        {
            return FMOD_System_GetOutputFormat(systemraw, ref samplerate, ref format, ref numchannels, ref bits, ref extradata);
        }
        public RESULT setMaxInputChannels    (int channels)
        {
            return FMOD_System_SetMaxInputChannels(systemraw, channels);
        }
        public RESULT getMaxInputChannels    (ref int channels)
        {
            return FMOD_System_GetMaxInputChannels(systemraw, ref channels);
        }
        public RESULT getNumDrivers          (ref int numdrivers)
        {
            return FMOD_System_GetNumDrivers(systemraw, ref numdrivers);
        }
        public RESULT getDriverName          (int id, StringBuilder name, int namelen)
        {
            return FMOD_System_GetDriverName(systemraw, id, name, namelen);
        }
        public RESULT getDriverCaps          (int id, ref CAPS caps, ref int minfrequency, ref int maxfrequency, ref SPEAKERMODE controlpanelspeakermode)
        {
            return FMOD_System_GetDriverCaps(systemraw, id, ref caps, ref minfrequency, ref maxfrequency, ref controlpanelspeakermode);
        }
        public RESULT setDriver              (int driver)
        {
            return FMOD_System_SetDriver(systemraw, driver);
        }
        public RESULT getDriver              (ref int driver)
        {
            return FMOD_System_GetDriver(systemraw, ref driver);
        }
        public RESULT setDSPBufferSize       (uint bufferlength, int numbuffers)
        {
            return FMOD_System_SetDSPBufferSize(systemraw, bufferlength, numbuffers);
        }
        public RESULT getDSPBufferSize       (ref uint bufferlength, ref int numbuffers)
        {
            return FMOD_System_GetDSPBufferSize(systemraw, ref bufferlength, ref numbuffers);
        }
        public RESULT setMinMaxHWChannels    (int min2d, int max2d, int min3d, int max3d)
        {
            return FMOD_System_SetMinMaxHWChannels(systemraw, min2d, max2d, min3d, max3d);
        }
        public RESULT setFileSystem          (FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize)
        {
            return FMOD_System_SetFileSystem(systemraw, useropen, userclose, userread, userseek, buffersize);
        }
        public RESULT attachFileSystem       (FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek)
        {
            return FMOD_System_AttachFileSystem(systemraw, useropen, userclose, userread, userseek);
        }
           
                                 
        // Plug-in support
        public RESULT setPluginPath          (string path)
        {
            return FMOD_System_SetPluginPath(systemraw, path);
        }
        public RESULT loadPlugin             (string filename, ref PLUGINTYPE plugintype, ref int index)
        {
            return FMOD_System_LoadPlugin(systemraw, filename, ref plugintype, ref index);
        }
        public RESULT getNumPlugins          (PLUGINTYPE plugintype, ref int numplugins)
        {
            return FMOD_System_GetNumPlugins(systemraw, plugintype, ref numplugins);
        }
        public RESULT getPluginInfo          (PLUGINTYPE plugintype, int index, StringBuilder name, int namelen, ref uint version)
        {
            return FMOD_System_GetPluginInfo(systemraw, plugintype, index, name, namelen, ref version);
        }
        public RESULT unloadPlugin           (PLUGINTYPE plugintype, int index)
        {
            return FMOD_System_UnloadPlugin(systemraw, plugintype, index);
        }


        // Init/Close 
        public RESULT init                   (int maxchannels, int maxsoftwarechannels, INITFLAG flags, IntPtr hwnd)
        {
            return FMOD_System_Init(systemraw, maxchannels, maxsoftwarechannels, flags, hwnd);
        }
        public RESULT close                  ()
        {
            return FMOD_System_Close(systemraw);
        }


        // General post-init system functions
        public RESULT update                 ()
        {
            return FMOD_System_Update(systemraw);
        }
        public RESULT setSpeakerMode         (SPEAKERMODE speakermode)
        {
            return FMOD_System_SetSpeakerMode(systemraw, speakermode);
        }
        public RESULT getSpeakerMode         (ref SPEAKERMODE speakermode)
        {
            return FMOD_System_GetSpeakerMode(systemraw, ref speakermode);
        }
        public RESULT setSpeakerPosition     (SPEAKER speaker, float x, float y)
        {
            return FMOD_System_SetSpeakerPosition(systemraw, speaker, x, y);
        }
        public RESULT getSpeakerPosition     (SPEAKER speaker, ref float x, ref float y)
        {
            return FMOD_System_GetSpeakerPosition(systemraw, speaker, ref x, ref y);
        }
        public RESULT setMasterVolume        (float volume)
        {
            return FMOD_System_SetMasterVolume(systemraw, volume);
        }
        public RESULT getMasterVolume        (ref float volume)
        {
            return FMOD_System_GetMasterVolume(systemraw, ref volume);
        }
                     
        public RESULT set3DSettings          (float dopplerscale, float distancefactor, float rolloffscale)
        {
            return FMOD_System_Set3DSettings(systemraw, dopplerscale, distancefactor, rolloffscale);
        }
        public RESULT get3DSettings          (ref float dopplerscale, ref float distancefactor, ref float rolloffscale)
        {
            return FMOD_System_Get3DSettings(systemraw, ref dopplerscale, ref distancefactor, ref rolloffscale);
        }
        public RESULT set3DNumListeners      (int numlisteners)
        {
            return FMOD_System_Set3DNumListeners(systemraw, numlisteners);
        }
        public RESULT get3DNumListeners      (ref int numlisteners)
        {
            return FMOD_System_Get3DNumListeners(systemraw, ref numlisteners);
        }
        public RESULT set3DListenerAttributes(int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_System_Set3DListenerAttributes(systemraw, listener, ref pos, ref vel, ref forward, ref up);
        }
        public RESULT get3DListenerAttributes(int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_System_Get3DListenerAttributes(systemraw, listener, ref pos, ref vel, ref forward, ref up);
        }

        public RESULT setStreamBufferSize    (uint decodebuffersize, TIMEUNIT decodebuffersizetype, uint filebuffersize, TIMEUNIT filebuffersizetype)
        {
            return FMOD_System_SetStreamBufferSize(systemraw, decodebuffersize, decodebuffersizetype, filebuffersize, filebuffersizetype);
        }
        public RESULT getStreamBufferSize    (ref uint decodebuffersize, ref TIMEUNIT decodebuffersizetype, ref uint filebuffersize, ref TIMEUNIT filebuffersizetype)
        {
            return FMOD_System_GetStreamBufferSize(systemraw, ref decodebuffersize, ref decodebuffersizetype, ref filebuffersize, ref filebuffersizetype);
        }


        // System information functions.
        public RESULT getVersion             (ref uint version)
        {
            return FMOD_System_GetVersion(systemraw, ref version);
        }
        public RESULT getOutputHandle        (ref IntPtr handle)
        {
            return FMOD_System_GetOutputHandle(systemraw, ref handle);
        }
        public RESULT getNumHWChannels       (ref int numhw3dchannels, ref int numhw2dchannels, ref int totalchannels)
        {
            return FMOD_System_GetNumHWChannels(systemraw, ref numhw3dchannels, ref numhw2dchannels, ref totalchannels);
        }
        public RESULT getChannelsPlaying     (ref int channels)
        {
            return FMOD_System_GetChannelsPlaying(systemraw, ref channels);
        }
        public RESULT getCPUUsage            (ref float dsp, ref float stream, ref float update, ref float total)
        {
            return FMOD_System_GetCPUUsage(systemraw, ref dsp, ref stream, ref update, ref total);
        }
        public RESULT getNumCDROMDrives      (ref int numdrives)
        {
            return FMOD_System_GetNumCDROMDrives(systemraw, ref numdrives);
        }
        public RESULT getCDROMDriveName      (int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen)
        {
            return FMOD_System_GetCDROMDriveName(systemraw, drive, drivename, drivenamelen, scsiname, scsinamelen, devicename, devicenamelen);
        }
        public RESULT getSpectrum            (float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_System_GetSpectrum(systemraw, spectrumarray, numvalues, channeloffset, windowtype);
        }
        public RESULT getWaveData            (float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_System_GetWaveData(systemraw, wavearray, numvalues, channeloffset);
        }


        // Sound/DSP/Channel creation and retrieval. 
        public RESULT createSound            (string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            RESULT result           = RESULT.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            try
            {
                result = FMOD_System_CreateSound(systemraw, name_or_data, mode, ref exinfo, ref soundraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }
            else
            {
                sound.setRaw(soundraw);
            }

            return result;
        }
        public RESULT createSound            (string name_or_data, MODE mode, ref Sound sound)
        {
            RESULT result           = RESULT.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            try
            {
                result = FMOD_System_CreateSound(systemraw, name_or_data, mode, 0, ref soundraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }
            else
            {
                sound.setRaw(soundraw);
            }

            return result;
        }
        public RESULT createStream            (string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref Sound sound)
        {
            RESULT result           = RESULT.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            try
            {
                result = FMOD_System_CreateStream(systemraw, name_or_data, mode, ref exinfo, ref soundraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }
            else
            {
                sound.setRaw(soundraw);
            }

            return result;
        }
        public RESULT createStream            (string name_or_data, MODE mode, ref Sound sound)
        {
            RESULT result           = RESULT.OK;
            IntPtr      soundraw    = new IntPtr();
            Sound       soundnew    = null;

            try
            {
                result = FMOD_System_CreateStream(systemraw, name_or_data, mode, 0, ref soundraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }
            else
            {
                sound.setRaw(soundraw);
            }

            return result;
        }
        public RESULT createDSP              (ref DSP_DESCRIPTION description, ref DSP dsp)
        {
            RESULT result = RESULT.OK;
            IntPtr dspraw = new IntPtr();
            DSP    dspnew = null;

            try
            {
                result = FMOD_System_CreateDSP(systemraw, ref description, ref dspraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }
                             
            return result;  
        }
        public RESULT createDSPByType          (DSP_TYPE type, ref DSP dsp)
        {
            RESULT result = RESULT.OK;
            IntPtr dspraw = new IntPtr();
            DSP    dspnew = null;

            try
            {
                result = FMOD_System_CreateDSPByType(systemraw, type, ref dspraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }
                             
            return result;  
        }
        public RESULT createDSPByIndex       (int index, ref DSP dsp)
        {
            RESULT result = RESULT.OK;
            IntPtr dspraw = new IntPtr();
            DSP    dspnew = null;

            try
            {
                result = FMOD_System_CreateDSPByIndex(systemraw, index, ref dspraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }
                             
            return result;  
        }
                       
        public RESULT playSound              (int channelid, Sound sound, bool paused, ref Channel channel)
        {
            RESULT result      = RESULT.OK;
            IntPtr      channelraw;
            Channel     channelnew  = null;

            if (channel != null)
            {
                channelraw = channel.getRaw();
            }
            else
            {
                channelraw  = new IntPtr();
            }

            try
            {
                result = FMOD_System_PlaySound(systemraw, channelid, sound.getRaw(), paused, ref channelraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (channel == null)
            {
                channelnew = new Channel();
                channelnew.setRaw(channelraw);
                channel = channelnew;
            }
            else
            {
                channel.setRaw(channelraw);
            }
                             
            return result;                                                                    
        }
        public RESULT playDSP                (int channelid, DSP dsp, bool paused, ref Channel channel)
        {
            RESULT result           = RESULT.OK;
            IntPtr      channelraw;
            Channel     channelnew  = null;

            if (channel != null)
            {
                channelraw = channel.getRaw();
            }
            else
            {
                channelraw  = new IntPtr();
            }

            try
            {
                result = FMOD_System_PlayDSP(systemraw, channelid, dsp.getRaw(), paused, ref channelraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (channel == null)
            {
                channelnew = new Channel();
                channelnew.setRaw(channelraw);
                channel = channelnew;
            }
            else
            {
                channel.setRaw(channelraw);
            }
                             
            return result;  
        }
        public RESULT getChannel             (int channelid, ref Channel channel)
        {
            RESULT result      = RESULT.OK;
            IntPtr      channelraw  = new IntPtr();
            Channel     channelnew  = null;

            try
            {
                result = FMOD_System_GetChannel(systemraw, channelid, ref channelraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (channel == null)
            {
                channelnew = new Channel();
                channelnew.setRaw(channelraw);
                channel = channelnew;
            }
            else
            {
                channel.setRaw(channelraw);
            }

            return result;
        }
     

        // Reverb api
        public RESULT setReverbProperties    (ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_SetReverbProperties(systemraw, ref prop);
        }
        public RESULT getReverbProperties    (ref REVERB_PROPERTIES prop)
        {
            return FMOD_System_GetReverbProperties(systemraw, ref prop);
        }
                                        
        
        // System level DSP access.
        public RESULT getDSPHead             (ref DSP dsp)
        {
            RESULT result   = RESULT.OK;
            IntPtr dspraw   = new IntPtr();
            DSP    dspnew   = null;

            try
            {
                result = FMOD_System_GetDSPHead(systemraw, ref dspraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (dsp == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                dsp = dspnew;
            }
            else
            {
                dsp.setRaw(dspraw);
            }

            return result;
        }
        public RESULT addDSP             (DSP dsp)
        {
            return FMOD_System_AddDSP(systemraw, dsp.getRaw());
        }
        public RESULT lockDSP            ()
        {
            return FMOD_System_LockDSP(systemraw);
        }
        public RESULT unlockDSP          ()
        {
            return FMOD_System_UnlockDSP(systemraw);
        }
                                            
        
        // Recording api
        public RESULT setRecordDriver        (int driver)
        {
            return FMOD_System_SetRecordDriver(systemraw, driver);
        }
        public RESULT getRecordDriver        (ref int driver)
        {
            return FMOD_System_GetRecordDriver(systemraw, ref driver);
        }
        public RESULT getRecordNumDrivers    (ref int numdrivers)
        {
            return FMOD_System_GetRecordNumDrivers(systemraw, ref numdrivers);
        }
        public RESULT getRecordDriverName    (int id, StringBuilder name, int namelen)
        {
            return FMOD_System_GetRecordDriverName(systemraw, id, name, namelen);
        }
 
        public RESULT getRecordPosition      (ref uint position)
        {
            return FMOD_System_GetRecordPosition(systemraw, ref position);
        }
        public RESULT recordStart            (Sound sound, bool loop)
        {
            return FMOD_System_RecordStart(systemraw, sound.getRaw(), loop);
        }
        public RESULT recordStop             ()
        {
            return FMOD_System_RecordStop(systemraw);
        }
        public RESULT isRecording            (ref bool recording)
        {
            return FMOD_System_IsRecording(systemraw, ref recording);
        }
         
      
        // Geometry api	
        public RESULT createGeometry		 (int maxPolygons, int maxVertices, ref Geometry geometryf)
        {
            RESULT result           = RESULT.OK;
            IntPtr      geometryraw    = new IntPtr();
            Geometry    geometrynew    = null;

            try
            {
                result = FMOD_System_CreateGeometry(systemraw, maxPolygons, maxVertices, ref geometryraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (geometryf == null)
            {
                geometrynew = new Geometry();
                geometrynew.setRaw(geometryraw);
                geometryf = geometrynew;
            }
            else
            {
                geometryf.setRaw(geometryraw);
            }

            return result;
        }
        public RESULT setGeometrySettings    (float maxWorldSize)
        {
            return FMOD_System_SetGeometrySettings(systemraw, maxWorldSize);
        }
        public RESULT getGeometrySettings    (ref float maxWorldSize)
        {
            return FMOD_System_GetGeometrySettings(systemraw, ref maxWorldSize);
        }
        public RESULT loadGeometry(IntPtr data, int dataSize, ref Geometry geometry)
        {
            RESULT result           = RESULT.OK;
            IntPtr      geometryraw    = new IntPtr();
            Geometry    geometrynew    = null;

            try
            {
                result = FMOD_System_LoadGeometry(systemraw, data, dataSize, ref geometryraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (geometry == null)
            {
                geometrynew = new Geometry();
                geometrynew.setRaw(geometryraw);
                geometry = geometrynew;
            }
            else
            {
                geometry.setRaw(geometryraw);
            }

            return result;
        }

  
        // Network functions
        public RESULT setProxy               (string proxy)
        {
            return FMOD_System_SetProxy(systemraw, proxy);
        }
        public RESULT getProxy               (StringBuilder proxy, int proxylen)
        {
            return FMOD_System_GetProxy(systemraw, proxy, proxylen);
        }
       
                                     
        // Userdata set/get                         
        public RESULT setUserData            (IntPtr userdata)
        {
            return FMOD_System_SetUserData(systemraw, userdata);
        }
        public RESULT getUserData            (ref IntPtr userdata)
        {
            return FMOD_System_GetUserData(systemraw, ref userdata);
        }



        #region importfunctions

        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Release                (IntPtr system);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetOutput              (IntPtr system, OUTPUTTYPE output);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetOutput              (IntPtr system, ref OUTPUTTYPE output);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetOutputPlugin        (IntPtr system, int index);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetOutputPlugin        (IntPtr system, ref int index);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetOutputFormat        (IntPtr system, int samplerate, SOUND_FORMAT format, int numchannels, IntPtr extradata);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetOutputFormat        (IntPtr system, ref int samplerate, ref SOUND_FORMAT format, ref int numchannels, ref int bits, ref IntPtr extradata);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetMaxInputChannels    (IntPtr system, int channels);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetMaxInputChannels    (IntPtr system, ref int channels);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetNumDrivers          (IntPtr system, ref int numdrivers);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetDriverName          (IntPtr system, int id, StringBuilder name, int namelen);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetDriverCaps          (IntPtr system, int id, ref CAPS caps, ref int minfrequency, ref int maxfrequency, ref SPEAKERMODE controlpanelspeakermode);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetDriver              (IntPtr system, int driver);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetDriver              (IntPtr system, ref int driver);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetDSPBufferSize       (IntPtr system, uint bufferlength, int numbuffers);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetDSPBufferSize       (IntPtr system, ref uint bufferlength, ref int numbuffers);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetMinMaxHWChannels    (IntPtr system, int min2d, int max2d, int min3d, int max3d);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetFileSystem          (IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek, int buffersize);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_AttachFileSystem       (IntPtr system, FILE_OPENCALLBACK useropen, FILE_CLOSECALLBACK userclose, FILE_READCALLBACK userread, FILE_SEEKCALLBACK userseek);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetPluginPath          (IntPtr system, string path);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_LoadPlugin             (IntPtr system, string filename, ref PLUGINTYPE plugintype, ref int index);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetNumPlugins          (IntPtr system, PLUGINTYPE plugintype, ref int numplugins);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetPluginInfo          (IntPtr system, PLUGINTYPE plugintype, int index, StringBuilder name, int namelen, ref uint version);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_UnloadPlugin           (IntPtr system, PLUGINTYPE plugintype, int index);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Init                   (IntPtr system, int maxchannels, int maxsoftwarechannels, INITFLAG flags, IntPtr hwnd);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Close                  (IntPtr system);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Update                 (IntPtr system);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetSpeakerMode         (IntPtr system, SPEAKERMODE speakermode);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetSpeakerMode         (IntPtr system, ref SPEAKERMODE speakermode);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetSpeakerPosition     (IntPtr system, SPEAKER speaker, float x, float y);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetSpeakerPosition     (IntPtr system, SPEAKER speaker, ref float x, ref float y);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetMasterVolume        (IntPtr system, float volume);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetMasterVolume        (IntPtr system, ref float volume);
        [DllImport (VERSION.dll)]                       
        private static extern RESULT FMOD_System_Set3DSettings          (IntPtr system, float dopplerscale, float distancefactor, float rolloffscale);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Get3DSettings          (IntPtr system, ref float dopplerscale, ref float distancefactor, ref float rolloffscale);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Set3DNumListeners      (IntPtr system, int numlisteners);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Get3DNumListeners      (IntPtr system, ref int numlisteners);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Set3DListenerAttributes(IntPtr system, int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_Get3DListenerAttributes(IntPtr system, int listener, ref VECTOR pos, ref VECTOR vel, ref VECTOR forward, ref VECTOR up);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetFileBufferSize      (IntPtr system, int sizebytes);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetFileBufferSize      (IntPtr system, ref int sizebytes);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetStreamBufferSize    (IntPtr system, uint decodebuffersize, TIMEUNIT decodebuffersizetype, uint filebuffersize, TIMEUNIT filebuffersizetype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetStreamBufferSize    (IntPtr system, ref uint decodebuffersize, ref TIMEUNIT decodebuffersizetype, ref uint filebuffersize, ref TIMEUNIT filebuffersizetype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetVersion             (IntPtr system, ref uint version);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetOutputHandle        (IntPtr system, ref IntPtr handle);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetNumHWChannels       (IntPtr system, ref int numhw3dchannels, ref int numhw2dchannels, ref int totalchannels);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetChannelsPlaying     (IntPtr system, ref int channels);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetCPUUsage            (IntPtr system, ref float dsp, ref float stream, ref float update, ref float total);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetNumCDROMDrives      (IntPtr system, ref int numdrives);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetCDROMDriveName      (IntPtr system, int drive, StringBuilder drivename, int drivenamelen, StringBuilder scsiname, int scsinamelen, StringBuilder devicename, int devicenamelen);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetSpectrum            (IntPtr system, [MarshalAs(UnmanagedType.LPArray)]float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetWaveData            (IntPtr system, [MarshalAs(UnmanagedType.LPArray)]float[] wavearray, int numvalues, int channeloffset);
        [DllImport (VERSION.dll)]   
        private static extern RESULT FMOD_System_CreateSound            (IntPtr system, string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_CreateStream           (IntPtr system, string name_or_data, MODE mode, ref CREATESOUNDEXINFO exinfo, ref IntPtr sound);
        [DllImport (VERSION.dll)]   
        private static extern RESULT FMOD_System_CreateSound            (IntPtr system, string name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_CreateStream           (IntPtr system, string name_or_data, MODE mode, int exinfo, ref IntPtr sound);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_CreateDSP              (IntPtr system, ref DSP_DESCRIPTION description, ref IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_CreateDSPByType        (IntPtr system, DSP_TYPE type, ref IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_CreateDSPByIndex       (IntPtr system, int index, ref IntPtr dsp);
        [DllImport (VERSION.dll)]                 
        private static extern RESULT FMOD_System_PlaySound              (IntPtr system, int channelid, IntPtr sound, bool paused, ref IntPtr channel);
        [DllImport (VERSION.dll)]
        public static extern RESULT FMOD_System_PlayDSP                 (IntPtr system, int channelid, IntPtr dsp, bool paused, ref IntPtr channel);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetChannel             (IntPtr system, int channelid, ref IntPtr channel);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetReverbProperties    (IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetReverbProperties    (IntPtr system, ref REVERB_PROPERTIES prop);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetDSPHead             (IntPtr system, ref IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_AddDSP                 (IntPtr system, IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_LockDSP                (IntPtr system);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_UnlockDSP              (IntPtr system);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetRecordDriver        (IntPtr system, int driver);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetRecordDriver        (IntPtr system, ref int driver);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetRecordNumDrivers    (IntPtr system, ref int numdrivers);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetRecordDriverName    (IntPtr system, int id, StringBuilder name, int namelen);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetRecordPosition      (IntPtr system, ref uint position);
        [DllImport (VERSION.dll)]  
        private static extern RESULT FMOD_System_RecordStart            (IntPtr system, IntPtr sound, bool loop);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_RecordStop             (IntPtr system);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_IsRecording            (IntPtr system, ref bool recording);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_CreateGeometry         (IntPtr system, int maxPolygons, int maxVertices, ref IntPtr geometryf);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetGeometrySettings    (IntPtr system, float maxWorldSize);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetGeometrySettings    (IntPtr system, ref float maxWorldSize);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_LoadGeometry           (IntPtr system, IntPtr data, int dataSize, ref IntPtr geometry);
        [DllImport (VERSION.dll)]               
        private static extern RESULT FMOD_System_SetProxy               (IntPtr system, string proxy);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetProxy               (IntPtr system, StringBuilder proxy, int proxylen);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_SetUserData            (IntPtr system, IntPtr userdata);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_System_GetUserData            (IntPtr system, ref IntPtr userdata);

        #endregion

        #region wrapperinternal
        
        private IntPtr systemraw;

        public void setRaw(IntPtr system)
        {
            systemraw = new IntPtr();

            systemraw = system;
        }

        public IntPtr getRaw()
        {
            return systemraw;
        }

        #endregion
    }
    

    /*
        'Sound' API
    */
    public class Sound
    {
        public RESULT release                 ()
        {
            return FMOD_Sound_Release(soundraw);
        }
        public RESULT getSystemObject         (ref System system)
        {
            RESULT result   = RESULT.OK;
            IntPtr systemraw   = new IntPtr();
            System systemnew   = null;

            try
            {
                result = FMOD_Sound_GetSystemObject(soundraw, ref systemraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (system == null)
            {
                systemnew = new System();
                systemnew.setRaw(systemraw);
                system = systemnew;
            }
            else
            {
                system.setRaw(systemraw);
            }
            return result;  
        }
                     

        public RESULT @lock                   (uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2)
        {
            return FMOD_Sound_Lock(soundraw, offset, length, ref ptr1, ref ptr2, ref len1, ref len2);
        }
        public RESULT unlock                  (IntPtr ptr1,  IntPtr ptr2, uint len1, uint len2)
        {
            return FMOD_Sound_Unlock(soundraw, ptr1, ptr2, len1, len2);
        }
        public RESULT setDefaults             (float frequency, float volume, float pan, ref float levels, int priority)
        {
            return FMOD_Sound_SetDefaults(soundraw, frequency, volume, pan, ref levels, priority);
        }
        public RESULT getDefaults             (ref float frequency, ref float volume, ref float pan,  ref float levels, ref int priority)
        {
            return FMOD_Sound_GetDefaults(soundraw, ref frequency, ref volume, ref pan, ref levels, ref priority);
        }
        public RESULT setVariations           (float frequencyvar, float volumevar, float panvar)
        {
            return FMOD_Sound_SetVariations(soundraw, frequencyvar, volumevar, panvar);
        }
        public RESULT getVariations           (ref float frequencyvar, ref float volumevar, ref float panvar)
        {
            return FMOD_Sound_GetVariations(soundraw, ref frequencyvar, ref volumevar, ref panvar); 
        }
        public RESULT set3DMinMaxDistance     (float min, float max)
        {
            return FMOD_Sound_Set3DMinMaxDistance(soundraw, min, max);
        }
        public RESULT get3DMinMaxDistance     (ref float min, ref float max)
        {
            return FMOD_Sound_Get3DMinMaxDistance(soundraw, ref min, ref max);
        }
        public RESULT setSubSound             (int index, Sound subsound)
        {
            IntPtr subsoundraw = subsound.getRaw();

            return FMOD_Sound_SetSubSound(soundraw, index, subsoundraw);
        }
        public RESULT getSubSound             (int index, ref Sound subsound)
        {
            RESULT result       = RESULT.OK;
            IntPtr subsoundraw  = new IntPtr();
            Sound  subsoundnew  = null;

            try
            {
                result = FMOD_Sound_GetSubSound(soundraw, index, ref subsoundraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (subsound == null)
            {
                subsoundnew = new Sound();
                subsoundnew.setRaw(subsoundraw);
                subsound = subsoundnew;
            }
            else
            {
                subsound.setRaw(subsoundraw);
            }

            return result;
        }
        public RESULT getName                 (StringBuilder name, int namelen)
        {
            return FMOD_Sound_GetName(soundraw, name, namelen);
        }
        public RESULT getLength               (ref uint length, TIMEUNIT lengthtype)
        {
            return FMOD_Sound_GetLength(soundraw, ref length, lengthtype);
        }
        public RESULT getFormat               (ref SOUND_TYPE type, ref SOUND_FORMAT format, ref int channels, ref int bits)
        {
            return FMOD_Sound_GetFormat(soundraw, ref type, ref format, ref channels, ref bits);
        }
        public RESULT getNumSubSounds         (ref int numsubsounds)
        {
            return FMOD_Sound_GetNumSubSounds(soundraw, ref numsubsounds);
        }
        public RESULT getNumTags              (ref int numtags, ref int numtagsupdated)
        {
            return FMOD_Sound_GetNumTags(soundraw, ref numtags, ref numtagsupdated);
        }
        public RESULT getTag                  (string name, int index, ref TAG tag)
        {
            return FMOD_Sound_GetTag(soundraw, name, index, ref tag);
        }
        public RESULT getOpenState            (ref OPENSTATE openstate, ref uint percentbuffered, ref bool starving)
        {
            return FMOD_Sound_GetOpenState(soundraw, ref openstate, ref percentbuffered, ref starving);
        }
        public RESULT readData                (ref IntPtr buffer, uint lenbytes, ref uint read)
        {
            return FMOD_Sound_ReadData(soundraw, ref buffer, lenbytes, ref read);
        }
        public RESULT seekData                (uint pcm)
        {
            return FMOD_Sound_SeekData(soundraw, pcm);
        }
        public RESULT setMode                 (MODE mode)
        {
            return FMOD_Sound_SetMode(soundraw, mode);
        }
        public RESULT getMode                 (ref MODE mode)
        {
            return FMOD_Sound_GetMode(soundraw, ref mode);
        }
        public RESULT setLoopCount            (int loopcount)
        {
            return FMOD_Sound_SetLoopCount(soundraw, loopcount);
        }
        public RESULT getLoopCount            (ref int loopcount)
        {
            return FMOD_Sound_GetLoopCount(soundraw, ref loopcount);
        }
        public RESULT setLoopPoints           (uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Sound_SetLoopPoints(soundraw, loopstart, loopstarttype, loopend, loopendtype);
        }
        public RESULT getLoopPoints           (ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Sound_GetLoopPoints(soundraw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }


        public RESULT setUserData             (IntPtr userdata)
        {
            return FMOD_Sound_SetUserData(soundraw, userdata);
        }
        public RESULT getUserData             (ref IntPtr userdata)
        {
            return FMOD_Sound_GetUserData(soundraw, ref userdata);
        }



        #region importfunctions

        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_Release                 (IntPtr sound);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetSystemObject         (IntPtr sound, ref IntPtr system);
        [DllImport (VERSION.dll)]                   
        private static extern RESULT FMOD_Sound_Lock                   (IntPtr sound, uint offset, uint length, ref IntPtr ptr1, ref IntPtr ptr2, ref uint len1, ref uint len2);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_Unlock                  (IntPtr sound, IntPtr ptr1,  IntPtr ptr2, uint len1, uint len2);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_SetDefaults             (IntPtr sound, float frequency, float volume, float pan, ref float levels, int priority);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetDefaults             (IntPtr sound, ref float frequency, ref float volume, ref float pan,  ref float levels, ref int priority);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_SetVariations           (IntPtr sound, float frequencyvar, float volumevar, float panvar);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetVariations           (IntPtr sound, ref float frequencyvar, ref float volumevar, ref float panvar);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_Set3DMinMaxDistance     (IntPtr sound, float min, float max);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_Get3DMinMaxDistance     (IntPtr sound, ref float min, ref float max);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_SetSubSound             (IntPtr sound, int index, IntPtr subsound);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetSubSound             (IntPtr sound, int index, ref IntPtr subsound);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetName                 (IntPtr sound, StringBuilder name, int namelen);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetLength               (IntPtr sound, ref uint length, TIMEUNIT lengthtype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetFormat               (IntPtr sound, ref SOUND_TYPE type, ref SOUND_FORMAT format, ref int channels, ref int bits);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetNumSubSounds         (IntPtr sound, ref int numsubsounds);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetNumTags              (IntPtr sound, ref int numtags, ref int numtagsupdated);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetTag                  (IntPtr sound, string name, int index, ref TAG tag);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetOpenState            (IntPtr sound, ref OPENSTATE openstate, ref uint percentbuffered, ref bool starving);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_ReadData                (IntPtr sound, ref IntPtr buffer, uint lenbytes, ref uint read);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_SeekData                (IntPtr sound, uint pcm);
        [DllImport (VERSION.dll)]                   
        private static extern RESULT FMOD_Sound_SetMode                 (IntPtr sound, MODE mode);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetMode                 (IntPtr sound, ref MODE mode);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_SetLoopCount            (IntPtr sound, int loopcount);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetLoopCount            (IntPtr sound, ref int loopcount);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_SetLoopPoints           (IntPtr sound, uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetLoopPoints           (IntPtr sound, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
        [DllImport (VERSION.dll)]                                        
        private static extern RESULT FMOD_Sound_SetUserData             (IntPtr sound, IntPtr userdata);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Sound_GetUserData             (IntPtr sound, ref IntPtr userdata);

        #endregion

        #region wrapperinternal

        private IntPtr soundraw;

        public void setRaw(IntPtr sound)
        {
            soundraw = new IntPtr();
            soundraw = sound;
        }

        public IntPtr getRaw()
        {
            return soundraw;
        }

        #endregion
    }


    /*
        'Channel' API
    */
    public class Channel
    {
        public RESULT getSystemObject       (ref System system)
        {
            RESULT result   = RESULT.OK;
            IntPtr systemraw   = new IntPtr();
            System systemnew   = null;

            try
            {
                result = FMOD_Channel_GetSystemObject(channelraw, ref systemraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (system == null)
            {
                systemnew = new System();
                systemnew.setRaw(systemraw);
                system = systemnew;
            }
            else
            {
                system.setRaw(systemraw);
            }

            return result;  
        }


        public RESULT stop                  ()
        {
            return FMOD_Channel_Stop(channelraw);
        }
        public RESULT setPaused             (bool paused)
        {
            return FMOD_Channel_SetPaused(channelraw, paused);
        }
        public RESULT getPaused             (ref bool paused)
        {
            return FMOD_Channel_GetPaused(channelraw, ref paused);
        }
        public RESULT setVolume             (float volume)
        {
            return FMOD_Channel_SetVolume(channelraw, volume);
        }
        public RESULT getVolume             (ref float volume)
        {
            return FMOD_Channel_GetVolume(channelraw, ref volume);
        }
        public RESULT setFrequency          (float frequency)
        {
            return FMOD_Channel_SetFrequency(channelraw, frequency);
        }
        public RESULT getFrequency          (ref float frequency)
        {
            return FMOD_Channel_GetFrequency(channelraw, ref frequency);
        }
        public RESULT setPan                (float pan)
        {
            return FMOD_Channel_SetPan(channelraw, pan);
        }
        public RESULT getPan                (ref float pan)
        {
            return FMOD_Channel_GetPan(channelraw, ref pan);
        }
        public RESULT setDelay              (uint startdelay, uint enddelay)
        {
            return FMOD_Channel_SetDelay(channelraw, startdelay, enddelay);
        }
        public RESULT getDelay              (ref uint startdelay, ref uint enddelay)
        {
            return FMOD_Channel_GetDelay(channelraw, ref startdelay, ref enddelay);
        }
        public RESULT setSpeakerMix         (float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright)
        {
            return FMOD_Channel_SetSpeakerMix(channelraw, frontleft, frontright, center, lfe, backleft, backright, sideleft, sideright);
        }
        public RESULT getSpeakerMix         (ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright)
        {
            return FMOD_Channel_GetSpeakerMix(channelraw, ref frontleft, ref frontright, ref center, ref lfe, ref backleft, ref backright, ref sideleft, ref sideright);
        }
        public RESULT setSpeakerLevels      (SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_Channel_SetSpeakerLevels(channelraw, speaker, levels, numlevels);
        }
        public RESULT getSpeakerLevels      (SPEAKER speaker, float[] levels, int numlevels)
        {
            return FMOD_Channel_GetSpeakerLevels(channelraw, speaker, levels, numlevels);
        }
        public RESULT setMute               (bool mute)
        {
            return FMOD_Channel_SetMute(channelraw, mute);
        }
        public RESULT getMute               (ref bool mute)
        {
            return FMOD_Channel_GetMute(channelraw, ref mute);
        }
        public RESULT setPriority           (int priority)
        {
            return FMOD_Channel_SetPriority(channelraw, priority);
        }
        public RESULT getPriority           (ref int priority)
        {
            return FMOD_Channel_GetPriority(channelraw, ref priority);
        }
        public RESULT setPosition           (uint position, TIMEUNIT postype)
        {
            return FMOD_Channel_SetPosition(channelraw, position, postype);
        }
        public RESULT getPosition           (ref uint position, TIMEUNIT postype)
        {
            return FMOD_Channel_GetPosition(channelraw, ref position, postype);
        }
        public RESULT set3DAttributes       (ref VECTOR pos, ref VECTOR vel)
        {
            return FMOD_Channel_Set3DAttributes(channelraw, ref pos, ref vel);
        }
        public RESULT get3DAttributes       (ref VECTOR pos, ref VECTOR vel)
        {
            return FMOD_Channel_Get3DAttributes(channelraw, ref pos, ref vel);
        }
        public RESULT set3DMinMaxDistance   (float mindistance, float maxdistance)
        {
            return FMOD_Channel_Set3DMinMaxDistance(channelraw, mindistance, maxdistance);
        }
        public RESULT get3DMinMaxDistance   (ref float mindistance, ref float maxdistance)
        {
            return FMOD_Channel_Get3DMinMaxDistance(channelraw, ref mindistance, ref maxdistance);
        }
        public RESULT set3DConeSettings     (float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            return FMOD_Channel_Set3DConeSettings(channelraw, insideconeangle, outsideconeangle, outsidevolume);
        }
        public RESULT get3DConeSettings     (ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume)
        {
            return FMOD_Channel_Get3DConeSettings(channelraw, ref insideconeangle, ref outsideconeangle, ref outsidevolume);
        }
        public RESULT set3DConeOrientation  (ref VECTOR orientation)
        {
            return FMOD_Channel_Set3DConeOrientation(channelraw, ref orientation);
        }
        public RESULT get3DConeOrientation  (ref VECTOR orientation)
        {
            return FMOD_Channel_Get3DConeOrientation(channelraw, ref orientation);
        }


        public RESULT setReverbProperties   (ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_Channel_SetReverbProperties(channelraw, ref prop);
        }
        public RESULT getReverbProperties   (ref REVERB_CHANNELPROPERTIES prop)
        {
            return FMOD_Channel_GetReverbProperties(channelraw, ref prop);
        }
        public RESULT setVolumeAbsolute     (float volume)
        {
            return FMOD_Channel_SetVolumeAbsolute(channelraw, volume);
        }
        public RESULT isPlaying             (ref bool isplaying)
        {
            return FMOD_Channel_IsPlaying(channelraw, ref isplaying);
        }
        public RESULT isVirtual             (ref bool isvirtual)
        {
            return FMOD_Channel_IsVirtual(channelraw, ref isvirtual);
        }
        public RESULT getAudibility(ref float audibility)
        {
            return FMOD_Channel_GetAudibility(channelraw, ref audibility);
        }
        public RESULT getCurrentSound       (ref Sound sound)
        {
            RESULT result      = RESULT.OK;
            IntPtr soundraw    = new IntPtr();
            Sound  soundnew    = null;

            try
            {
                result = FMOD_Channel_GetCurrentSound(channelraw, ref soundraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (sound == null)
            {
                soundnew = new Sound();
                soundnew.setRaw(soundraw);
                sound = soundnew;
            }
            else
            {
                sound.setRaw(soundraw);
            }

            return result;  
        }
        public RESULT getSpectrum           (float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype)
        {
            return FMOD_Channel_GetSpectrum(channelraw, spectrumarray, numvalues, channeloffset, windowtype);
        }
        public RESULT getWaveData           (float[] wavearray, int numvalues, int channeloffset)
        {
            return FMOD_Channel_GetWaveData(channelraw, wavearray, numvalues, channeloffset);
        }
        public RESULT getLevels             (float[] levels, ref int numlevels)
        {
            return FMOD_Channel_GetLevels(channelraw, levels, ref numlevels);
        }
        public RESULT setCallback           (CHANNEL_CALLBACKTYPE type, CHANNEL_CALLBACK callback, int command)
        {
            return FMOD_Channel_SetCallback(channelraw, type, callback, command);
        }
        public RESULT getDSPHead            (ref DSP dsp)
        {
            RESULT result      = RESULT.OK;
            IntPtr dspraw      = new IntPtr();
            DSP    dspnew      = null;

            try
            {
                result = FMOD_Channel_GetDSPHead(channelraw, ref dspraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            dspnew = new DSP();
            dspnew.setRaw(dspraw);
            dsp = dspnew;

            return result; 
        }
        public RESULT addDSP(DSP dsp)
        {
            return FMOD_Channel_AddDSP(channelraw, dsp.getRaw());
        }
        public RESULT setOcclusion          (float directOcclusion, float reverbOcclusion)
        {
            return FMOD_Channel_SetOcclusion(channelraw, directOcclusion, reverbOcclusion);
        }
        public RESULT getOcclusion          (ref float directOcclusion, ref float reverbOcclusion)
        {
            return FMOD_Channel_GetOcclusion(channelraw, ref directOcclusion, ref reverbOcclusion);
        }
         
            
        public RESULT setMode               (MODE mode)
        {
            return FMOD_Channel_SetMode(channelraw, mode);
        }
        public RESULT getMode               (ref MODE mode)
        {
            return FMOD_Channel_GetMode(channelraw, ref mode);
        }
        public RESULT setLoopCount          (int loopcount)
        {
            return FMOD_Channel_SetLoopCount(channelraw, loopcount);
        }
        public RESULT getLoopCount          (ref int loopcount)
        {
            return FMOD_Channel_GetLoopCount(channelraw, ref loopcount);
        }
        public RESULT setLoopPoints         (uint loopstart, TIMEUNIT loopstarttype, uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Channel_SetLoopPoints(channelraw, loopstart, loopstarttype, loopend, loopendtype);
        }
        public RESULT getLoopPoints         (ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype)
        {
            return FMOD_Channel_GetLoopPoints(channelraw, ref loopstart, loopstarttype, ref loopend, loopendtype);
        }


        public RESULT setUserData           (IntPtr userdata)
        {
            return FMOD_Channel_SetUserData(channelraw, userdata);
        }
        public RESULT getUserData           (ref IntPtr userdata)
        {
            return FMOD_Channel_GetUserData(channelraw, ref userdata);
        }



        #region importfunctions

        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetSystemObject       (IntPtr channel, ref IntPtr system);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_Stop                  (IntPtr channel);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetPaused             (IntPtr channel, bool paused);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetPaused             (IntPtr channel, ref bool paused);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetVolume             (IntPtr channel, float volume);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetVolume             (IntPtr channel, ref float volume);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetFrequency          (IntPtr channel, float frequency);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetFrequency          (IntPtr channel, ref float frequency);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetPan                (IntPtr channel, float pan);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetPan                (IntPtr channel, ref float pan);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetDelay              (IntPtr channel, uint startdelay, uint enddelay);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetDelay              (IntPtr channel, ref uint startdelay, ref uint enddelay);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetSpeakerMix         (IntPtr channel, float frontleft, float frontright, float center, float lfe, float backleft, float backright, float sideleft, float sideright);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetSpeakerMix         (IntPtr channel, ref float frontleft, ref float frontright, ref float center, ref float lfe, ref float backleft, ref float backright, ref float sideleft, ref float sideright);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetSpeakerLevels      (IntPtr channel, SPEAKER speaker, float[] levels, int numlevels);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetSpeakerLevels      (IntPtr channel, SPEAKER speaker, [MarshalAs(UnmanagedType.LPArray)]float[] levels, int numlevels);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetMute               (IntPtr channel, bool mute);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetMute               (IntPtr channel, ref bool mute);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetPriority           (IntPtr channel, int priority);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetPriority           (IntPtr channel, ref int priority);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_Set3DAttributes       (IntPtr channel, ref VECTOR pos, ref VECTOR vel);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_Get3DAttributes       (IntPtr channel, ref VECTOR pos, ref VECTOR vel);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_Set3DMinMaxDistance   (IntPtr channel, float mindistance, float maxdistance);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_Get3DMinMaxDistance   (IntPtr channel, ref float mindistance, ref float maxdistance);
        [DllImport (VERSION.dll)]        
        private static extern RESULT FMOD_Channel_Set3DConeSettings     (IntPtr channel, float insideconeangle, float outsideconeangle, float outsidevolume);
        [DllImport (VERSION.dll)] 
        private static extern RESULT FMOD_Channel_Get3DConeSettings     (IntPtr channel, ref float insideconeangle, ref float outsideconeangle, ref float outsidevolume);
        [DllImport (VERSION.dll)] 
        private static extern RESULT FMOD_Channel_Set3DConeOrientation  (IntPtr channel, ref VECTOR orientation);
        [DllImport (VERSION.dll)] 
        private static extern RESULT FMOD_Channel_Get3DConeOrientation  (IntPtr channel, ref VECTOR orientation);
        [DllImport (VERSION.dll)] 
        private static extern RESULT FMOD_Channel_SetReverbProperties   (IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetReverbProperties   (IntPtr channel, ref REVERB_CHANNELPROPERTIES prop);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetVolumeAbsolute     (IntPtr channel, float volume);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_IsPlaying             (IntPtr channel, ref bool isplaying);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_IsVirtual             (IntPtr channel, ref bool isvirtual);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetAudibility         (IntPtr channel, ref float audibility);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetCurrentSound       (IntPtr channel, ref IntPtr sound);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetSpectrum           (IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] spectrumarray, int numvalues, int channeloffset, DSP_FFT_WINDOW windowtype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetWaveData           (IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] wavearray, int numvalues, int channeloffset);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetLevels             (IntPtr channel, [MarshalAs(UnmanagedType.LPArray)] float[] levels, ref int numlevels);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetCallback           (IntPtr channel, CHANNEL_CALLBACKTYPE type, CHANNEL_CALLBACK callback, int command);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetPosition           (IntPtr channel, uint position, TIMEUNIT postype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetPosition           (IntPtr channel, ref uint position, TIMEUNIT postype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetDSPHead            (IntPtr channel, ref IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_AddDSP                (IntPtr channel, IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetOcclusion          (IntPtr channel, float directOcclusion, float reverbOcclusion);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetOcclusion          (IntPtr channel, ref float directOcclusion, ref float reverbOcclusion);
        [DllImport (VERSION.dll)]                       
        private static extern RESULT FMOD_Channel_SetMode               (IntPtr channel, MODE mode);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetMode               (IntPtr channel, ref MODE mode);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetLoopCount          (IntPtr channel, int loopcount);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetLoopCount          (IntPtr channel, ref int loopcount);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_SetLoopPoints         (IntPtr channel, uint  loopstart, TIMEUNIT loopstarttype, uint  loopend, TIMEUNIT loopendtype);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetLoopPoints         (IntPtr channel, ref uint loopstart, TIMEUNIT loopstarttype, ref uint loopend, TIMEUNIT loopendtype);
        [DllImport (VERSION.dll)]                                        
        private static extern RESULT FMOD_Channel_SetUserData           (IntPtr channel, IntPtr userdata);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Channel_GetUserData           (IntPtr channel, ref IntPtr userdata);
    
        #endregion
        
        #region wrapperinternal

        private IntPtr channelraw;

        public void setRaw(IntPtr channel)
        {
            channelraw = new IntPtr();

            channelraw = channel;
        }

        public IntPtr getRaw()
        {
            return channelraw;
        }

        #endregion
    }


    /*
        'DSP' API
    */
    public class DSP
    {
        public RESULT release                   ()
        {
            return FMOD_DSP_Release(dspraw);
        }
        public RESULT getSystemObject           (ref System system)
        {
            RESULT result         = RESULT.OK;
            IntPtr systemraw      = new IntPtr();
            System systemnew      = null;

            try
            {
                result = FMOD_DSP_GetSystemObject(dspraw, ref systemraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (system == null)
            {
                systemnew = new System();
                systemnew.setRaw(dspraw);
                system = systemnew;
            }
            else
            {
                system.setRaw(systemraw);
            }

            return result;             
        }
                     

        public RESULT addInput                  (DSP target)
        {
            return FMOD_DSP_AddInput(dspraw, target.getRaw());
        }
        public RESULT disconnectFrom            (DSP target)
        {
            return FMOD_DSP_DisconnectFrom(dspraw, target.getRaw());
        }
        public RESULT remove                    ()
        {
            return FMOD_DSP_Remove(dspraw);
        }
        public RESULT getNumInputs              (ref int numinputs)
        {
            return FMOD_DSP_GetNumInputs(dspraw, ref numinputs);
        }
        public RESULT getNumOutputs             (ref int numoutputs)
        {
            return FMOD_DSP_GetNumOutputs(dspraw, ref numoutputs);
        }
        public RESULT getInput                  (int index, ref DSP input)
        {
            RESULT result      = RESULT.OK;
            IntPtr dspraw      = new IntPtr();
            DSP    dspnew      = null;

            try
            {
                result = FMOD_DSP_GetInput(dspraw, index, ref dspraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (input == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                input = dspnew;
            }
            else
            {
                input.setRaw(dspraw);
            }

            return result; 
        }
        public RESULT getOutput                 (int index, ref DSP output)
        {
            RESULT result      = RESULT.OK;
            IntPtr dspraw      = new IntPtr();
            DSP    dspnew      = null;

            try
            {
                result = FMOD_DSP_GetOutput(dspraw, index, ref dspraw);
            }
            catch
            {
                result = RESULT.ERR_INVALID_PARAM;
            }
            if (result != RESULT.OK)
            {
                return result;
            }

            if (output == null)
            {
                dspnew = new DSP();
                dspnew.setRaw(dspraw);
                output = dspnew;
            }
            else
            {
                output.setRaw(dspraw);
            }

            return result; 
        }
        public RESULT setInputMix               (int index, float volume)
        {
            return FMOD_DSP_SetInputMix(dspraw, index, volume);
        }
        public RESULT getInputMix               (int index, ref float volume)
        {
            return FMOD_DSP_GetInputMix(dspraw, index, ref volume);
        }
        public RESULT setActive                 (bool active)
        {
            return FMOD_DSP_SetActive(dspraw, active);
        }
        public RESULT getActive                 (ref bool active)
        {
            return FMOD_DSP_GetActive(dspraw, ref active);
        }
        public RESULT setBypass                 (bool bypass)
        {
            return FMOD_DSP_SetBypass(dspraw, bypass);
        }
        public RESULT getBypass                 (ref bool bypass)
        {
            return FMOD_DSP_GetBypass(dspraw, ref bypass);
        }
        public RESULT reset                     ()
        {
            return FMOD_DSP_Reset(dspraw);
        }

                     
        public RESULT setParameter              (int index, float val)
        {
            return FMOD_DSP_SetParameter(dspraw, index, val);
        }
        public RESULT getParameter              (int index, ref float val, StringBuilder valuestr, int valuestrlen)
        {
            return FMOD_DSP_GetParameter(dspraw, index, ref val, valuestr, valuestrlen);
        }
        public RESULT getNumParameters          (ref int numparams)
        {
            return FMOD_DSP_GetNumParameters(dspraw, ref numparams);
        }
        public RESULT getParameterInfo          (int index, StringBuilder name, StringBuilder label, StringBuilder description, int descriptionlen, ref float min, ref float max)
        {
            return FMOD_DSP_GetParameterInfo(dspraw, index, name, label, description, descriptionlen, ref min, ref max);
        }
        public RESULT showConfigDialog          (IntPtr hwnd, bool show)
        {
            return FMOD_DSP_ShowConfigDialog          (dspraw, hwnd, show);
        }


        public RESULT getInfo                   (StringBuilder name, ref uint version, ref int channels, ref int configwidth, ref int configheight)
        {
            return FMOD_DSP_GetInfo(dspraw, name, ref version, ref channels, ref configwidth, ref configheight);
        }
        public RESULT setDefaults               (float frequency, float volume, float pan, ref float levels, int priority)
        {
            return FMOD_DSP_SetDefaults(dspraw, frequency, volume, pan, ref levels, priority);
        }
        public RESULT getDefaults(ref float frequency, ref float volume, ref float pan, ref float levels, ref int priority)
        {
            return FMOD_DSP_GetDefaults(dspraw, ref frequency, ref volume, ref pan, ref levels, ref priority);
        }


        public RESULT setUserData               (IntPtr userdata)
        {
            return FMOD_DSP_SetUserData(dspraw, userdata);
        }
        public RESULT getUserData               (ref IntPtr userdata)
        {
            return FMOD_DSP_GetUserData(dspraw, ref userdata);
        }



        #region importfunctions

        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_Release                   (IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetSystemObject           (IntPtr dsp, ref IntPtr system);
        [DllImport (VERSION.dll)]                   
        private static extern RESULT FMOD_DSP_AddInput                  (IntPtr dsp, IntPtr target);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_DisconnectFrom            (IntPtr dsp, IntPtr target);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_Remove                    (IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetNumInputs              (IntPtr dsp, ref int numinputs);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetNumOutputs             (IntPtr dsp, ref int numoutputs);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetInput                  (IntPtr dsp, int index, ref IntPtr input);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetOutput                 (IntPtr dsp, int index, ref IntPtr output);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetInputMix               (IntPtr dsp, int index, float volume);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetInputMix               (IntPtr dsp, int index, ref float volume);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetActive                 (IntPtr dsp, bool active);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetActive                 (IntPtr dsp, ref bool active);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetBypass                 (IntPtr dsp, bool bypass);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetBypass                 (IntPtr dsp, ref bool bypass);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_Reset                     (IntPtr dsp);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetParameter              (IntPtr dsp, int index, float val);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetParameter              (IntPtr dsp, int index, ref float val, StringBuilder valuestr, int valuestrlen);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetNumParameters          (IntPtr dsp, ref int numparams);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetParameterInfo          (IntPtr dsp, int index, StringBuilder name, StringBuilder label, StringBuilder description, int descriptionlen, ref float min, ref float max);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_ShowConfigDialog          (IntPtr dsp, IntPtr hwnd, bool show);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetInfo                   (IntPtr dsp, StringBuilder name, ref uint version, ref int channels, ref int configwidth, ref int configheight);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_SetDefaults               (IntPtr dsp, float frequency, float volume, float pan, ref float levels, int priority);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetDefaults               (IntPtr dsp, ref float frequency, ref float volume, ref float pan, ref float levels, ref int priority);
        [DllImport (VERSION.dll)]                   
        private static extern RESULT FMOD_DSP_SetUserData               (IntPtr dsp, IntPtr userdata);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_DSP_GetUserData               (IntPtr dsp, ref IntPtr userdata);

        #endregion

        #region wrapperinternal

        private IntPtr dspraw;

        public void setRaw(IntPtr dsp)
        {
            dspraw = new IntPtr();

            dspraw = dsp;
        }

        public IntPtr getRaw()
        {
            return dspraw;
        }

        #endregion
    }


    /*
        'Geometry' API
    */
    public class Geometry
    {
        public RESULT release               ()
        {
            return FMOD_Geometry_Release(geometryraw);
        }       
        public RESULT addPolygon			(float directOcclusion, float reverbOcclusion, bool doubleSided, int numVertices, ref VECTOR vertices, ref int polygonIndex)
        {
            return FMOD_Geometry_AddPolygon(geometryraw, directOcclusion, reverbOcclusion, doubleSided, numVertices, ref vertices, ref polygonIndex);
        }


        public RESULT getNumPolygons		(ref int numPolygons)
        {
            return FMOD_Geometry_GetNumPolygons(geometryraw, ref numPolygons);
        }
        public RESULT getMaxPolygons		(ref int maxPolygons, ref int maxVertices)
        {
            return FMOD_Geometry_GetMaxPolygons(geometryraw, ref maxPolygons, ref maxVertices);
        }
        public RESULT getPolygonNumVertices	(int polygonIndex, ref int numVertices)
        {
            return FMOD_Geometry_GetPolygonNumVertices(geometryraw, polygonIndex, ref numVertices);
        }
        public RESULT setPolygonVertex		(int polygonIndex, int vertexIndex, ref VECTOR vertex)
        {
            return FMOD_Geometry_SetPolygonVertex(geometryraw, polygonIndex, vertexIndex, ref vertex);
        }
        public RESULT getPolygonVertex		(int polygonIndex, int vertexIndex, ref VECTOR vertex)
        {
            return FMOD_Geometry_GetPolygonVertex(geometryraw, polygonIndex, vertexIndex, ref vertex);
        }
        public RESULT setPolygonAttributes	(int polygonIndex, float directOcclusion, float reverbOcclusion, bool doubleSided)
        {
            return FMOD_Geometry_SetPolygonAttributes(geometryraw, polygonIndex, directOcclusion, reverbOcclusion, doubleSided);
        }
        public RESULT getPolygonAttributes	(int polygonIndex, ref float directOcclusion, ref float reverbOcclusion, ref bool doubleSided)
        {
            return FMOD_Geometry_GetPolygonAttributes(geometryraw, polygonIndex, ref directOcclusion, ref reverbOcclusion, ref doubleSided);
        }
        public RESULT flush                 ()
        {
            return FMOD_Geometry_Flush(geometryraw);
        }

        public RESULT setActive             (bool active)
        {
            return FMOD_Geometry_SetActive  (geometryraw, active);
        }
        public RESULT getActive             (ref bool active)
        {
            return FMOD_Geometry_GetActive  (geometryraw, ref active);
        }
        public RESULT setRotation			(ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_Geometry_SetRotation(geometryraw, ref forward, ref up);
        }
        public RESULT getRotation			(ref VECTOR forward, ref VECTOR up)
        {
            return FMOD_Geometry_GetRotation(geometryraw, ref forward, ref up);
        }
        public RESULT setPosition			(ref VECTOR position)
        {
            return FMOD_Geometry_SetPosition(geometryraw, ref position);
        }
        public RESULT getPosition			(ref VECTOR position)
        {
            return FMOD_Geometry_GetPosition(geometryraw, ref position);
        }
        public RESULT setScale				(ref VECTOR scale)
        {
            return FMOD_Geometry_SetScale(geometryraw, ref scale);
        }
        public RESULT getScale				(ref VECTOR scale)
        {
            return FMOD_Geometry_GetScale(geometryraw, ref scale);
        }
        public RESULT save                  (IntPtr data, ref int datasize)
        {
            return FMOD_Geometry_Save(geometryraw, data, ref datasize);
        }


        public RESULT setUserData               (IntPtr userdata)
        {
            return FMOD_Geometry_SetUserData(geometryraw, userdata);
        }
        public RESULT getUserData               (ref IntPtr userdata)
        {
            return FMOD_Geometry_GetUserData(geometryraw, ref userdata);
        }



        #region importfunctions

        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_Release   (IntPtr geometry);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_AddPolygon           (IntPtr geometry, float directOcclusion, float reverbOcclusion, bool doubleSided, int numVertices, ref VECTOR vertices, ref int polygonIndex);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetNumPolygons       (IntPtr geometry, ref int numPolygons);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetMaxPolygons       (IntPtr geometry, ref int maxPolygons, ref int maxVertices);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetPolygonNumVertices(IntPtr geometry, int polygonIndex, ref int numVertices);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetPolygonVertex     (IntPtr geometry, int polygonIndex, int vertexIndex, ref VECTOR vertex);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetPolygonVertex     (IntPtr geometry, int polygonIndex, int vertexIndex, ref VECTOR vertex);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetPolygonAttributes (IntPtr geometry, int polygonIndex, float directOcclusion, float reverbOcclusion, bool doubleSided);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetPolygonAttributes (IntPtr geometry, int polygonIndex, ref float directOcclusion, ref float reverbOcclusion, ref bool doubleSided);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_Flush                (IntPtr geometry);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetActive                    (IntPtr gemoetry, bool active);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetActive                    (IntPtr gemoetry, ref bool active);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetRotation          (IntPtr geometry, ref VECTOR forward, ref VECTOR up);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetRotation          (IntPtr geometry, ref VECTOR forward, ref VECTOR up);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetPosition          (IntPtr geometry, ref VECTOR position);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetPosition          (IntPtr geometry, ref VECTOR position);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_SetScale             (IntPtr geometry, ref VECTOR scale);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetScale             (IntPtr geometry, ref VECTOR scale);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_Save                 (IntPtr geometry, IntPtr data, ref int datasize);
        [DllImport (VERSION.dll)]                   
        private static extern RESULT FMOD_Geometry_SetUserData          (IntPtr geometry, IntPtr userdata);
        [DllImport (VERSION.dll)]
        private static extern RESULT FMOD_Geometry_GetUserData          (IntPtr geometry, ref IntPtr userdata);

        #endregion

        #region wrapperinternal

        private IntPtr geometryraw;

        public void setRaw(IntPtr geometry)
        {
            geometryraw = new IntPtr();

            geometryraw = geometry;
        }

        public IntPtr getRaw()
        {
            return geometryraw;
        }

        #endregion
    }
}