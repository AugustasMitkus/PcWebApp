import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import '../index.css';
import Table from "react-bootstrap/Table"


const MemberGroup : React.FC = () => {
    const { groupId } = useParams();
    const [groupName, setGroupName] = useState<string>("");
    useEffect(() => {
            //fetches a group and assigns its title/name
            const fetchGroup = async () => {
                try {
                    const response = await fetch(`http://localhost:5109/groups/${groupId}`)
                    if (response.ok) {
                        const data = await response.json();
                        setGroupName(data.name);

                    }
    
                    else {
                    const errorData = await response.json();
                    console.error(errorData.error)
                }
                }
                catch (error:any){
                    console.error("Error:", error.message);
                }
            };
        fetchGroup();
    }, [groupName,groupId]);
    return (
        <main className="container">
            <center>
                <h2 className="title">{groupName}</h2>
            </center>
        </main>
    )
}

export default MemberGroup