<xsl:stylesheet version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:IMSpoor="http://www.prorail.nl/IMSpoor"
  xmlns:gml="http://www.opengis.net/gml"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
  xmlns:M2MTsdk="M2MT:SDK">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>
  <xsl:template match="/">
    <eulynxDataPrepInterface xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://dataprep.eulynx.eu/schema/Generic/1.0">
        <id xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
        <hasDataContainer>
            <id xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
            <ownsDataPrepEntities>
                <id xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />

                <!-- BufferStops -->
                <xsl:for-each select="//IMSpoor:InitialSituation/IMSpoor:RailInfrastructure/IMSpoor:RailImplementation/IMSpoor:Junctions/IMSpoor:BufferStop">
                  <ownsTrackAsset xmlns="http://dataprep.eulynx.eu/schema/Signalling/1.0" xsi:type="BufferStop">
                    <id xmlns="http://www.railsystemmodel.org/schemas/Common/1.2">
                      <xsl:value-of select="@puic"/>
                    </id>
                    <xsl:element name="refersToRsmVehicleStop">
                      <xsl:attribute name="ref">
                        <xsl:value-of select="M2MTsdk:GetID(@puic,'VehicleStop')" />
                      </xsl:attribute>
                    </xsl:element>
                    <!-- <refersToRsmVehicleStop ref="a94c3d30-fc38-46c2-a567-921f0e5072a9" /> -->
                    <stopsVehicleAtLocation xsi:nil="true" />
                    <isOfBufferStopType>
                      <xsl:value-of select="@bufferstopType" />
                    </isOfBufferStopType>
                  </ownsTrackAsset>
                </xsl:for-each>
            </ownsDataPrepEntities>
            <ownsRsmEntities>
                <id xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />

                <!-- StopLocations for BufferStops -->
                <xsl:for-each select="//IMSpoor:InitialSituation/IMSpoor:RailInfrastructure/IMSpoor:RailImplementation/IMSpoor:Junctions/IMSpoor:BufferStop">
                    <ownsVehicleStop>
                        <id xmlns="http://www.railsystemmodel.org/schemas/Common/1.2">
                          <!-- Moet nog! a94c3d30-fc38-46c2-a567-921f0e5072a9 -->
                          <xsl:value-of select="M2MTsdk:GetID(@puic,'VehicleStop')" />
                        </id>
                        <longname xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                        <!-- <name xmlns="http://www.railsystemmodel.org/schemas/Common/1.2">
                            <xsl:value-of select="@name"/>
                        </name> -->
                        <xsl:element name="locations" namespace="http://www.railsystemmodel.org/schemas/NetEntity/1.2">
                          <xsl:attribute name="ref">
                            <xsl:value-of select="M2MTsdk:GetID(@puic,'usesLocation')" />
                          </xsl:attribute>
                        </xsl:element>
                        <!-- <locations ref="Moet nog! 06cf6ad0-4a7d-4f74-afe9-13240fc69654" xmlns="http://www.railsystemmodel.org/schemas/NetEntity/1.2" /> -->
                    </ownsVehicleStop>
                </xsl:for-each>

                <usesTopography>
                    <id xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                    <usesPositioningSystem>
                        <id xmlns="http://www.railsystemmodel.org/schemas/Common/1.2">
                          <xsl:value-of select="M2MTsdk:GetID('EPSG:28992,usesPositioningSystem')" />
                        </id>
                        <longname xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                        <name xmlns="http://www.railsystemmodel.org/schemas/Common/1.2">EPSG:28992</name>
                    </usesPositioningSystem>

                    <!-- StopLocations coordinates for BufferStops -->
                    <xsl:for-each select="//IMSpoor:InitialSituation/IMSpoor:RailInfrastructure/IMSpoor:RailImplementation/IMSpoor:Junctions/IMSpoor:BufferStop">
                      <xsl:variable name="puic" select="@puic" />
                        <xsl:for-each select="//IMSpoor:InitialSituation/IMSpoor:StopLocations">
                          <xsl:choose>
                            <xsl:when test="@ID = $puic">
                              <usesPositioningSystemCoordinate xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" xsi:type="GeographicCoordinate">
                                <id>
                                  <!-- 19e26e06-72d9-477c-9687-1010b305d021 -->
                                  <xsl:value-of select="M2MTsdk:GetID(@ID,'usesPositioningSystemCoordinate')" />
                                </id>
                                <longname xsi:nil="true" />
                                <name xsi:nil="true" />
                                <xsl:element name="positioningSystem">
                                  <xsl:attribute name="ref">
                                    <xsl:value-of select="M2MTsdk:GetID('EPSG:28992,usesPositioningSystem')" />
                                  </xsl:attribute>
                                </xsl:element>
                                <!-- <positioningSystem ref="329c5ad3-a3b9-47b7-8163-8647f08c6696" /> -->
                                <elevation>
                                  <xsl:value-of select="substring-after(substring-after(IMSpoor:StopLocation/IMSpoor:GeographicLocation/gml:Point/gml:coordinates/text(),','),',')"/>
                                </elevation>
                                <latitude>
                                  <xsl:value-of select="substring-before(substring-after(IMSpoor:StopLocation/IMSpoor:GeographicLocation/gml:Point/gml:coordinates/text(),','),',')"/>
                                </latitude>
                                <longitude>
                                  <xsl:value-of select="substring-before(substring-after(IMSpoor:StopLocation/IMSpoor:GeographicLocation/gml:Point/gml:coordinates/text(),','),',')"/>
                                </longitude>
                              </usesPositioningSystemCoordinate>
                            </xsl:when>
                          </xsl:choose>
                        </xsl:for-each>
                    </xsl:for-each>

                    <!-- BaseLocations for StopLocations of BufferStops -->
                    <xsl:for-each select="//IMSpoor:InitialSituation/IMSpoor:RailInfrastructure/IMSpoor:RailImplementation/IMSpoor:Junctions/IMSpoor:BufferStop">
                      <xsl:variable name="puic" select="@puic" />
                      <usesLocation xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" xsi:type="SpotLocation">
                          <id>
                            <!-- f5611c39-3e32-4809-9a36-6b0f5b8123a0 -->
                            <xsl:value-of select="M2MTsdk:GetID(@puic,'usesLocation')" />
                          </id>
                          <associatedNetElements>
                            <isLocatedToSide xsi:nil="true" />
                            <!-- <netElement ref="d6c70652-3512-6556-b270-c603bfd7a09e" /> -->
                            <xsl:for-each select="//IMSpoor:InitialSituation/IMSpoor:RailInfrastructure/IMSpoor:RailTopology/IMSpoor:MicroLinks/IMSpoor:MicroLink">
                              <xsl:choose>
                                <xsl:when test="IMSpoor:ToMicroNode/@nodeRef = $puic">
                                  <xsl:element name="netElement">
                                    <xsl:attribute name="ref">
                                      <xsl:value-of select="M2MTsdk:GetID(concat($puic, IMSpoor:FromMicroNode/@nodeRef),'usesNetElement')" />
                                    </xsl:attribute>
                                  </xsl:element>
                                </xsl:when>
                                <xsl:when test="IMSpoor:FromMicroNode/@nodeRef = $puic">
                                  <xsl:element name="netElement">
                                    <xsl:attribute name="ref">
                                      <xsl:value-of select="M2MTsdk:GetID(concat(IMSpoor:ToMicroNode/@nodeRef, $puic),'usesNetElement')" />
                                    </xsl:attribute>
                                  </xsl:element>
                                </xsl:when>
                              </xsl:choose>
                            </xsl:for-each>
                          </associatedNetElements>
                          <xsl:element name="coordinates">
                            <xsl:attribute name="ref">
                              <xsl:value-of select="M2MTsdk:GetID(@puic,'usesPositioningSystemCoordinate')" />
                            </xsl:attribute>
                          </xsl:element>
                          <!-- <coordinates ref="617db3d3-cf7d-4f02-8ee8-3aa043caa5f7" /> -->
                      </usesLocation>
                    </xsl:for-each>

                </usesTopography>

                <!-- topology -->
                <usesTrackTopology>
                  <id xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                  <xsl:for-each select="//IMSpoor:InitialSituation/IMSpoor:RailInfrastructure/IMSpoor:RailTopology/IMSpoor:MicroLinks/IMSpoor:MicroLink">
                    <xsl:variable name="puicFrom" select="IMSpoor:FromMicroNode/@nodeRef" />
                    <xsl:variable name="puicTo" select="IMSpoor:ToMicroNode/@nodeRef" />
                    <usesNetElement>
                      <id xmlns="http://www.railsystemmodel.org/schemas/Common/1.2">
                        <xsl:value-of select="M2MTsdk:GetID(concat($puicTo,  $puicFrom),'usesNetElement')" />
                      </id>
                      <longname xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                      <name xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                    </usesNetElement>
                  </xsl:for-each>
                    <!-- <xsl:for-each select="//IMSpoor:InitialSituation/IMSpoor:RailInfrastructure/IMSpoor:RailTopology/IMSpoor:MicroLinks/IMSpoor:MicroLink">
                    <usesPositionedRelation>
                      <id xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                      <longname xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                      <name xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" /> -->
                      <!-- <xsl:when test="IMSpoor:FromMicroNode/@nodeRef != ''"> -->
                        <!-- <xsl:element name="elementA">
                          <xsl:attribute name="ref">
                            <xsl:value-of select="M2MTsdk:GetID(IMSpoor:FromMicroNode/@nodeRef,'usesNetElement')" />
                          </xsl:attribute>
                        </xsl:element> -->
                      <!-- </xsl:when> -->
                      <!-- <xsl:when test="IMSpoor:FromMicroNode/@nodeRef != ''"> -->
                        <!-- <xsl:element name="elementB">
                          <xsl:attribute name="ref">
                            <xsl:value-of select="M2MTsdk:GetID(IMSpoor:ToMicroNode/@nodeRef,'usesNetElement')" />
                          </xsl:attribute>
                        </xsl:element> -->
                      <!-- </xsl:when> -->
                      <!-- <elementA ref="658c7469-004e-525a-a124-c0c752d8c5d5" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                      <elementB ref="7a2b1c77-4860-e95a-81a8-413943e8cd39" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" /> -->
                      <!-- <navigability xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                      <positionOnA xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                      <positionOnB xsi:nil="true" xmlns="http://www.railsystemmodel.org/schemas/Common/1.2" />
                    </usesPositionedRelation>
                  </xsl:for-each> -->
                </usesTrackTopology>
            </ownsRsmEntities>
        </hasDataContainer>
        <hasTimeStamp xsi:nil="true" />
        <toolName xsi:nil="true" />
        <toolVersion xsi:nil="true" />
    </eulynxDataPrepInterface>
  </xsl:template>
</xsl:stylesheet>
