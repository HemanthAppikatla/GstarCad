Imports Microsoft.Win32

Class MainWindow

    Dim cadHelper As GstarCadHelper
    Public Sub New()
        InitializeComponent()
        cadHelper = New GstarCadHelper()
    End Sub

    ''' <summary>
    ''' Loads the respective cad file from local directory into Cad Environment
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_LoadOnClick(sender As Object, e As RoutedEventArgs)
        Dim fileDialog As New OpenFileDialog With {
            .Title = "Please select G star Cad file",
            .Filter = "DWG|*.dwg"
        }
        Dim dialogResult As Boolean? = fileDialog.ShowDialog()
        If dialogResult = True Then
            Dim selectedFile As String = fileDialog.FileName
            If Not String.IsNullOrEmpty(selectedFile) Then
                TxtBox_InputFile.Text = selectedFile
            End If
        Else
            MessageBox.Show("Cancelled")
        End If
    End Sub

    Private Sub ButtonShowAllLayers_OnClick(sender As Object, e As RoutedEventArgs)
        'get all the layers info of the Document
        Dim allLayers As List(Of String) = cadHelper.GetAllLayers()
        'display the layers info 
        LayersInfo.ItemsSource = allLayers
    End Sub

    Private Sub ButtonEnableLayers_OnClick(sender As Object, e As RoutedEventArgs)
        If LayersInfo.SelectedItems.Count > 0 Then
            Dim selectedLayers As List(Of String) = New List(Of String) ' = CType(LayersInfo.SelectedItems, List(Of String))
            For Each item As Object In LayersInfo.SelectedItems
                Dim itemName As String = item.ToString
                selectedLayers.Add(item)
            Next

            cadHelper.EnableOrDisableLayers(selectedLayers, True)
            MessageBox.Show("Enabled the selected Layers")
        End If
        'reset the UI settings
        'CheckBox_SelectAllLayers.IsChecked = False
    End Sub

    Private Sub ButtonDisableLayers_OnClick(sender As Object, e As RoutedEventArgs)
        If LayersInfo.SelectedItems.Count > 0 Then
            Dim selectedLayers As List(Of String) = New List(Of String)
            For Each item As Object In LayersInfo.SelectedItems
                Dim itemName As String = item.ToString
                selectedLayers.Add(item)
            Next
            cadHelper.EnableOrDisableLayers(selectedLayers, False)
            MessageBox.Show("Disabled Selected Layers")
        End If
        'reset the UI settings
        'CheckBox_SelectAllLayers.IsChecked = False
    End Sub

    'Performs save operation  
    Private Sub ButtonSave_OnClick(sender As Object, e As RoutedEventArgs)
        cadHelper.SaveDoc()
    End Sub

    Private Sub CheckBox_SelectAllLayers_Click(sender As Object, e As RoutedEventArgs)
        If (CheckBox_SelectAllLayers.IsChecked) Then
            LayersInfo.SelectAll()
        Else
            LayersInfo.SelectedItems.Clear()
        End If

    End Sub


    ''' <summary>
    ''' Opens the selected file in Respective Cad application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_Open_Click(sender As Object, e As RoutedEventArgs)
        Dim filePath = TxtBox_InputFile.Text
        If Not String.IsNullOrEmpty(filePath) Then
            Dim loadResult As Boolean = cadHelper.Open(filePath)
            If loadResult Then
                MessageBox.Show("Loaded the Selected file in Cad")
            End If
        End If
    End Sub

    Private Sub ApplicationUi_OnLoad(sender As Object, e As RoutedEventArgs)

    End Sub


    ''' <summary>
    ''' Launchs Cad Application
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button_LaunchCad(sender As Object, e As RoutedEventArgs)
        cadHelper.LaunchGstarCad()
    End Sub
End Class
