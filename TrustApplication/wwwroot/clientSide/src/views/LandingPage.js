import React, { useState } from 'react';
import moment from 'moment';
import FilesDropbox from '../components/FilesDropbox';
import '../assets/css/views/LandingPage.css';

export default function LandingPage(props) {

    const [filesUploadedArray, setFilesUploadedArray] = useState();

    return (
        <div>
            <br/>
            <FilesDropbox setFilesUploadedArray={setFilesUploadedArray}/>
            <br/>
            <table>
                <thead>
                    <tr>
                        <th>Filename</th>
                        <th>Uploaded Date</th>
                    </tr>
                </thead>
                <tbody>
                    {!filesUploadedArray || filesUploadedArray.length <= 0 ?
                        <tr>
                            <td> </td>
                            <td> </td>
                        </tr>
                        :
                        filesUploadedArray.map((row) =>
                            <tr key={row.id}>
                                <td>{row.filename}</td>
                                <td>{moment(row.storedDateTime).format("DD/MM/YYYY HH:mm")}</td>
                            </tr>
                        )
                    }
                </tbody>
            </table>
        </div>
    )
}