Imports GrxCAD.Interop

Public Class GstarCadHelper
    Public Property CadApplication As GcadApplication
    Public CadDocument As GcadDocument = Nothing

    ''' <summary>
    ''' Connects to Gstar Cad application
    ''' </summary>
    ''' <returns></returns>
    Public Function LaunchGstarCad() As Boolean
        Try
            ' Starting Gstar Cad Application
            CadApplication = New GcadApplication()
            CadApplication.Visible = True

            Return CadApplication IsNot Nothing
        Catch ex As Exception
        End Try

        Return False
    End Function

    ''' <summary>
    ''' Load/Open model in GstarCad Environment
    ''' </summary>
    ''' <param name="modelPath">DXF/DWG file path to open</param>
    ''' <returns>True - Success, False - Failure</returns>
    Public Function Open(modelPath As String) As Boolean
        If CadApplication Is Nothing Then
            Return False
        End If

        Dim gCadDocs As GcadDocuments = CadApplication.Documents
        CadDocument = gCadDocs.Open(modelPath)

        Return CadDocument IsNot Nothing
    End Function

    ''' <summary>
    ''' Gets all the Layers information of current document
    ''' </summary>
    ''' <returns></returns>
    Public Function GetAllLayers() As List(Of String)
        Dim layerNames As New List(Of String)()
        If CadDocument Is Nothing Then
            Return layerNames
        End If
        ''get all the layers available in the current Document
        Dim gCadLayers As GcadLayers = CadDocument.Layers
        ''layers count
        Dim layerCount As Integer = gCadLayers.Count

        For idx As Integer = 0 To layerCount - 1
            Dim gCadLayer As GcadLayer = gCadLayers.Item(idx)
            Dim layerName As String = gCadLayer.Name
            layerNames.Add(layerName)
        Next
        ' Add code to retrieve layer names from GstarCAD

        Return layerNames
    End Function

    ''' <summary>
    ''' Enables/Disables the respected Layers based on User selection
    ''' </summary>
    ''' <param name="LayerNames">All the Layers Names To Update</param>
    ''' <param name="enable">Status to enable or disable
    '''                      True --> Enables the Layers
    '''                      False--> Disables the Layers</param>
    ''' <returns></returns>
    Public Function EnableOrDisableLayers(LayerNames As List(Of String), enable As Boolean) As Boolean
        'get the existing layers
        Dim gCadLayers As GcadLayers = CadDocument.Layers
        Dim layerCount As Integer = gCadLayers.Count

        For idx As Integer = 0 To layerCount - 1
            Dim gCadLayer As GcadLayer = gCadLayers.Item(idx)
            Dim layerName As String = gCadLayer.Name
            'check if we need to update the status of this layer
            If LayerNames.Contains(layerName) Then
                gCadLayer.LayerOn = enable
            End If
        Next

        CadDocument.Regen(GcRegenType.acAllViewports) 'regen after chnages
        Return True
    End Function


    ''' <summary>
    ''' Save active  CAD document with latest changes
    ''' </summary>
    Public Sub SaveDoc()
        If CadDocument IsNot Nothing Then
            CadDocument.Save()
        End If
    End Sub


End Class
