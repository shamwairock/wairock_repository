<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"  xmlns:yok="http://schemas.datacontract.org/2004/07/Yokogawa.IA.Dtm.Interface">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <html lang="en" xmlns="http://www.w3.org/1999/xhtml">
      <head>
        <meta charset="utf-8" />
      </head>
      <body>
        <table width="1000px" border="1px" style="text-align:left;border-collapse:collapse">
          <tr>
            <td>suhem</td>
          </tr>
          <xsl:for-each select="yok:HeaderInfo/yok:Rows/yok:Row">
            <tr>
              <!--<xsl:if test="position()=1">
                <td>
                  <xsl:attribute name="rowspan">
                    <xsl:value-of select="count(/HeaderInfo/Rows/Row)"/>
                  </xsl:attribute>
                  --><!--<xsl:value-of select="/HeaderInfo/DeviceImage/Value"/>--><!--
                </td>
              </xsl:if>-->
              <xsl:for-each select="yok:Cols/yok:Col">
                <td>
                  <xsl:value-of select="yok:ParameterItem/yok:Label"/>
                </td>
                <td>
                  <xsl:value-of select="yok:ParameterItem/yok:NodePath"/>
                </td>
              </xsl:for-each>
              <!--<xsl:if test="position()=1">
                <td>
                  <xsl:attribute name="rowspan">
                    <xsl:value-of select="count(/HeaderInfo/Rows/Row)"/>
                  </xsl:attribute>Company Image
                </td>
              </xsl:if>-->
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>
