#include "stdafx.h"
#include "Crypt.h"

#include "PacketInfo.h"

DLLFUNCTION int GetPacketLength( unsigned char *packet, int len )
{
	if ( pShared != NULL )
	{
		int packetLen = pShared->PacketTable[(unsigned char)packet[0]];

		if ( packetLen >= 0x4000 )
		{
			if ( len < 3 )
				return 0;
			else
				return packet[1]<<8 | packet[2];
		}
		else
		{
			return packetLen;
		}
	}
	else if ( len >= 3 )
	{
		//attempt to auto detect an undefined packet
		return packet[1]<<8 | packet[2];
	}
	else
	{
		return -1;
	}
}

DLLFUNCTION bool IsDynLength( BYTE packet )
{
	if ( pShared != NULL )
		return pShared->PacketTable[packet] >= 0x8000;
	else
		return true;
}
