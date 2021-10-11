import React, { useState } from 'react';
import '../assets/css/components/FilesDropbox.css';

export default function FilesDropbox(props) {

    // Check for the various File API support.
    if (window.File && window.FileReader && window.FileList && window.Blob) {
        // Great success! All the File APIs are supported.
    } else {
        alert('The File APIs are not fully supported in this browser.');
    }

    let dropBoxTextDefault = 'Browse for files \n (or drag and drop files)';
    if (/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)) {
        dropBoxTextDefault = 'Browse for files \n (press here)';
    }
    const dropBoxTextUploading = 'Uploading...';
    const [dropBoxText, setDropBoxText] = useState(dropBoxTextDefault);

    async function uploadFiles(filesToUploadData) {
        if (!filesToUploadData || filesToUploadData.length <= 0) {
            return;
        }
        await fetch('http://localhost:29323/api/Customer/PostFileUpload', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(filesToUploadData)
        })
            .then(response => response.json())
            .then(success => {
                if (success && success[0] && success[0].filename.length > 1) {
                    props.setFilesUploadedArray(success);
                }
            })
            .catch(error => console.log(error))
            .finally(() => {
                setDropBoxText(dropBoxTextDefault)
            })
    }

    function isAbleToUpload() {
        if (dropBoxText !== dropBoxTextDefault) {
            return false;
        }
        return true;
    }

    //Trigger a browse dialog via the hidden input
    function browseFilesToUpload() {
        if (!isAbleToUpload()) {
            return false;
        }
        let filesDropboxInputElement = document.getElementById('filesDropboxInputClick');
        if (filesDropboxInputElement) {
            filesDropboxInputElement.click();
        }
    }

    function readFileContent(file) {
        return new Promise((resolve, reject) => {
            let fileReader = new FileReader();
            fileReader.onload = function (response) {
                resolve(response.target.result);
            }
            fileReader.onerror = reject;
            fileReader.readAsDataURL(file); //settings to output base64 (increases filesize by 30%). Need to find a way to transfer binary without encoding
        })
    }

    //Changes made within filesDropbox component
    async function changeHandler(ev) {
        ev.preventDefault();
        if (!isAbleToUpload()) {
            return false;
        }
        setDropBoxText(dropBoxTextUploading);
        let filesArray = [];
        if (ev.dataTransfer && ev.dataTransfer.files.length > 0) {
            filesArray = ev.dataTransfer.files;
        }
        else if (ev.dataTransfer && ev.dataTransfer.items.length > 0) {
            filesArray = ev.dataTransfer.items;
        }
        else if (ev.target && ev.target.files.length > 0) {
            filesArray = ev.target.files;
        }
        if (filesArray && filesArray.length > 0) {
            let filesToUploadArray = [];
            for (let fileCount = 0; fileCount < filesArray.length; fileCount++) {
                let file = filesArray[fileCount];
                await readFileContent(file)
                    .then(fileContentBinary => filesToUploadArray[fileCount] = {
                        "filename": file.name,
                        "content": fileContentBinary
                    })
                    .catch(errorRes => console.log('Error: ' + errorRes));
            }
            uploadFiles(filesToUploadArray);
        }
    }

    function dragOverHandler(ev) {
        ev.preventDefault();
    }

    return (
        <div className={"filesDropbox"} id="filesDropboxInputDragAndDrop" encType="multipart/form-data" onDrop={changeHandler} onDragOver={dragOverHandler} onClick={browseFilesToUpload}>
            {dropBoxText} <br />
            <input type="file" id="filesDropboxInputClick" multiple onChange={changeHandler} hidden />
        </div>
    )
}