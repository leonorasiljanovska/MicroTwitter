export interface Post{
    id: string;
    username: string; 
    userId: string;
    content: string;
    createdAt: Date;
    imageUrl?: string;
}